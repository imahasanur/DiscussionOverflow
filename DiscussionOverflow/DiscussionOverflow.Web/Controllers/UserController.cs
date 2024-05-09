using Amazon.S3.Transfer;
using Amazon.S3;
using Autofac;
using DiscussionOverflow.Infrastructure.Membership;
using DiscussionOverflow.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Amazon;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DiscussionOverflow.Infrastructure;

namespace DiscussionOverflow.Web.Controllers
{

    public class UserController : Controller
    {

        private readonly ILifetimeScope _scope;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserController(ILifetimeScope scope,
            ILogger<AccountController> logger,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _scope = scope;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }



        [Authorize(Policy = "HandleVotePolicy")]
        public async Task<IActionResult> HandleVote(Guid id, string type, string postItem)
        {
            var model = new HandleVoteModel();
            model.Resolve(_scope);
            bool success = await model.UpdateVoteCountAsync(id, type, postItem);
            return Json(new { success});
        }



        public async Task<IActionResult> Questions()
        {
            var model = new ViewQuestionsModel();
            model.Resolve(_scope);
            var questions = await model.GetAllQuestionAsync();
            return View(questions);
        }


        public IActionResult CreateQuestion()
        {
            var model = new QuestionModel();
            model.Resolve(_scope);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "QuestionCreatePolicy")]
        public async Task<IActionResult> CreateQuestion(QuestionModel model)
        {


            if (ModelState.IsValid)
            {
                model.Resolve(_scope);
                var response = model.VerifyTags(model.Tags);
                if (!response)
                {
                    ModelState.AddModelError("Tags", "Tags should be comma seperated without white space and not more than 5 and less than 1");
                    _logger.LogWarning("Tags should be comma seperated and not more than 5 and less than 1");
                    return View(model);
                }

                await model.CreateQuestionAsync();
                return RedirectToAction("Questions", "User");

            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Model State is not valid Error Count: {ModelState.ErrorCount}");
                _logger.LogWarning("Model State is not valid");
            }


            return View(model);
        }


        public async Task<IActionResult> EditProfile()
        {
            var model = new EditProfileModel();
            model.Resolve(_scope);
            model = await model.GetUserByEmailAsync();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "EditProfilePolicy")]
        public async Task<IActionResult> EditProfile(EditProfileModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    // Retrieve AWS credentials from IConfiguration
                    var awsAccessKeyId = _configuration["aws-access-key"];
                    var awsSecretAccessKey = _configuration["aws-secret-access-key"];
                    var awsUrl = _configuration["s3-url"];
                    var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey);
                    var fileName = Path.GetFileName(model.ImageFile.FileName);
                    string? photoKey = null;

                    using (var client = new AmazonS3Client(awsCredentials, RegionEndpoint.USEast1)) // Specify your region
                    {
                        try
                        {
                            var transferUtility = new TransferUtility(client);

                            // Specify the S3 bucket name
                            string bucketName = _configuration["aws-bucket"];

                            // Generate a unique photo key (file path) based on file name and current timestamp
                            photoKey = $"image/{fileName}";

                            // Upload the photo to S3
                            await transferUtility.UploadAsync(model.ImageFile.OpenReadStream(), bucketName, photoKey);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError(string.Empty, ex.Message);
                            _logger.LogWarning(ex.Message);
                        }

                    }
                    if (photoKey != null)
                    {
                        model.S3Url = awsUrl + "/" + photoKey;
                        model.ImageFileName = fileName;
                        model.ImageFileSize = (int)model.ImageFile.Length;
                    }

                }

                model.Resolve(_scope);
                var response = await model.UpdateUserAsync();
                if (!response.Succeeded)
                {
                    var errors = response.Errors;
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        _logger.LogWarning(error.Description);
                    }

                }
                else
                {
                    return RedirectToAction("Profile", "User");
                }
            }
            ModelState.AddModelError(string.Empty, $"Model State is not valid Error Count: {ModelState.ErrorCount}");
            _logger.LogWarning("Model State is not valid");

            return View();
        }


		[Authorize(Policy = "ViewProfilePolicy")]
		public async Task<IActionResult> Profile()
        {
            var model = new ProfileModel();
            //var model = _scope.Resolve<ProfileModel>();
            model.Resolve(_scope);
            model = await model.GetProfileUserByEmailAsync();
            return View(model);

        }

        public IActionResult QuestionTag()
        {
            var model = new QuestionTagModel();
            model.Resolve(_scope);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> QuestionTag(QuestionTagModel model)
        {
            if (ModelState.IsValid)
            {
                model.Resolve(_scope);
                await model.GetSearchQuestionAsync();
                return View(model);
            }
            ModelState.AddModelError(string.Empty, "There is an error in input taking");
            _logger.LogWarning("There is Model error in QuestionTag Action");
            return View();
        }

        public async Task<IActionResult> QuestionDetails(Guid id)
        {
            var model = new QuestionDetailsModel();
            model.Resolve(_scope);
            model = await model.GetQuestionDeatails(id);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "HandleCommentPolicy")]
        public async Task<IActionResult> QuestionDetails(QuestionDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                model.Resolve(_scope);
                await model.PostResponseAsync();
                return RedirectToAction("QuestionDetails", "User", new { id = model.GetId });
            }
            ModelState.AddModelError(string.Empty, $"Model State is not valid Error Count: {ModelState.ErrorCount}");
            _logger.LogWarning("Model State is not valid . Some Input value is missing");
            return View(model);
        }

        public async  Task<IActionResult> Notification()
        {
            var model = new NotificationModel();
            model.Resolve(_scope);
            var notifications = await model.GetAllNotificationAsync();
            return View(notifications);
        }
    }
}
