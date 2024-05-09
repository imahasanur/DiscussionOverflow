# Discussion Overflow 

It is a AspNet MVC (v 7.0) Project. Project is designed using Domain Driven Design(DDD) & Clean Architecture(CL).

## Features

Functional:
1. Users can register and login. Email verification has been added.
2. Like in stack overflow, a user gains(1 Reputation) certain level ( every new user) can post questions and reply to questions.
3. Users can post questions.
4. Users can reply to a question.
5. Users can comment on questions.
6. Users can upvote/downvote questions.
7. Users can use markdown editors (used Simple Markdown Editor) to post code in questions and replies.
8. Users can upload profile pictures (Used Aws s3 Bucket).
9. There are tags in question through which users can search and view questions.
10. There is a point/badge system for rewarding members (based on Reputation point).
11. Upvote on User's Question/Answer from other user will get (+10) Reputation, in Downvote (-2). 
12. Users can view their own questions.
13. Users can view and edit their profile information.
14. Users can search questions through Question Title, Tags.
15. There are notifications for reply and comment in questions.

Non Functional:
1. Written unit tests (N Unit Test) for service classes.
2. Used claim based authorization to control user access to the features. .
3. Used clean architecture to maintain flexible design.
4. Added Pictured Number recaptcha in LogIn & Registration pages in forms.
5. Added both client side and server side validation in some pages to maintain security.
6. Used AWS bucket to make the project sustainable.
7. Integrated serilog logger and use exception handling for fault tolerance. 
8. Used autofac to make the code flexible.
9. Used entity framework, repository and unit of work pattern to make the project robust. 
10. Used bootstrap, fontawesome icon to make the application user friendly.
11. All migrations added including seed data to make the project maintainable. 



## Used Technology
 FrontEnd: Html, Bootstrap, Fontawesome, Javascript 
 
 Server side: C#, Asp Net MVC (v7.0), Entity FrameWork Core, NUnit Test 
 
 Database : SQL SERVER
 
 Tool : Visual Studio


## Need to Run the project
 1. Download Packages from NUGET Package Manager
 2. Open project in Visual Studio Build & Run
 3. Dot Net MVC core(v7)
 4. SQL server


## Connect with me
portfolio site[ahasanur-rahman.com](https://ahasanur-rahman.web.app/)

linkedin profile[linkedin.com/ahasanur-rahman](https://www.linkedin.com/in/ahasanur-rahman-a10925202/)


