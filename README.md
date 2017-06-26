# SecurePass
A client-server control access application.

# Why SecurePass ?


Security has been one of the most controversial topics for the last few decades, in particular in the computer science field. Considering security as the possibility to guarantee a secure access to a private and protected environment, the main problems are:
•	Classic authentication system (username + password) is easily hackable;
•	In a work environment, a badge based system to access the office does not guarantee the real identity of the person;
•	Working frauds are commonplace;
•	An autonomous access to a protect environment (houses, condominium, etc.) by a limited group of people is not easy without a human surveillance; 


Currently all these problems are not addressed by a unique, secure and integrated application. For this reason, our proposal is SecurePass.
In our project as case of use, we have developed a system that allows employees to access their work environment. The other problems mentioned above are easily addressed by this technology and its system architecture.

# What is SecurePass?

The product developed during the course is called SecurePass. 
SecurePass is: 
•	A product in charge to handle secure access to every kind of private and protected environment;
•	The way to protect your company or your house using a double key verification;
•	A client-server, web based application.

# SecurePass: How it works?

Use case: Employee access

Let’s see a typical case of use in the work environment.
1.	The employee inserts using the GUI on the touch screen its unique ID provided by the system administrator and stored in the database;
2.	The system checks if the identifier matches a real employee in the system database;
3.	After that, employee credentials are showed on the screen;
4.	Now the employee takes a photo pressing the user button framing its face for matching;
5.	The face is sent to the server that, using the Microsoft Cognitive service matches that photo with the sample stored in the database
a.	If the confidence coefficient is higher than a certain threshold, the user can access to the environment and the entry/exit  time is stored in the database;
b.	If the two photos don’t match, the user can retry for 3 times: after that the system send san email to the administrator with the system log file.

