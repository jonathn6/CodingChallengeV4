# CodingChallengeV4

This Contact Manager

The request was to develop a simple contact manager application using Microsoft ASP.NET MVC in visual studio.

As per my conversation with Ben, I was given a bit of leway on the requirements as I have had little practical coding experience with some of the requirements and/or supporting technologies.  

Challenges
1.  I have never developed an application which made use of Entity Framework to interface with the database.
2.  I have no experience with ajax calls/posts to pass data to and from an MVC controller.
3.  I have limited jquery experience.

Application features
1.  The application makes use of code first (as per the requirements) whereby I coded the table classes and used the Entity Framework wizard to establish a CRUDModule.edmx.  To test this, I would manually delete the dbo.Contacts and dbo.EmailAddresses tables.  There is code in Transactions.cs that can be commented out which, when commented out, will allow the system to continue using the database and populated tables.  When the code is active, the system will drop the database prior to accessing the tables and the Entity Framework will recreate them as needed.
2.  The Requirements of this project called for the use of the seed method to set at least one initial value.  The Deliverables of the project called for seeding at least 2 contact records.  I implemented a custom database initializer and seed the database with 2 records.
3.  The application makes use of dynamically generated grids to display all contact information as well as allowing the user to double click on a grid row to pop up a dialog which allows the user to edit the contact information.
4.  The application will alert the user (on an edit of data) if the entered data is valid or not, if the email address being updated exists for a different user, or if no changes are being made to the data.

Setup
There is no setup required by this application other that forking the git and running it.  The Entity Framework should create a local database (assuming the user has MS SQL Server installed on their local machine.  

Usability
When the application is launched, the user will be presented with a standard bootstrap enhanced menu structure.  These are the functions available to the user:
1.  Home - Will return the user to the inital home page of the application.
2.  Add A Contact - Will display a screen where the user will be able to enter contact information.  The user will be alerted if the data entered is not valid.  If the data is entered correctly, an ajax call is used to send the data to the application controller.  The controller will verify whether the email address entered is present in the database or not.  If the email address already exists, the user will receive a warning that the email address is already in use.  The Add A Contact screen will remain presented to the user.  If the email address was not in use, the data will be saved to the database and the user will be informed of the successful add of data.
3.  View All Contacts - This will simply place a grid on the screen and display all the contact infomation from the database.
4.  Update All Contacts - This will generate a grid of all contacts in the database.  If the user wished to update a contact, the user can simply double click on any data element.  The system will then pop up a jquery dialog and display the data of the contact the user wishes to update.  The same editing as on Add a User will be done on Update.  The main difference is the check on the email address as the email address (if not being changed) will be found on the database.  If the user tries to change the email address to an address that is already in the database for a different contact, the user will be alerted.
