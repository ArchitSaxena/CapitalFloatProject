# CapitalFloatProject

In this Repository I have built a Web API with ASP.Net Core. I have created a **MySql DB** on **Google Cloud Platform** and used **Swagger** to describe RESTful APIs expressed using JSON.

The Project contains 2 Controller classes -
  1) PersonsController
  2) StudentsController

Each of the controller has **GET/POST/PUT and DELETE** HTTP Methods to perform CRUD operations on the tables of the Database.

There are 3 tables created in the Database: CapitalFloat - 
  1) **Persons** - To store the information of the Person like ID, Name, Address, City, etc
  2) **Student** - To store the Student information like ID, course, Dob, etc
  3) **StudentContactInfo** - To store the contact details of the student like PhoneNo, Email, FathersName, Address, etc.

There is a Models folder in the Project which contains the model classes **(Persons, Student, StudentContactInfo, StudentInfo)** of all these tables.

There is a DataAccess folder which contains the DataContext class **(CapitalFloatDataContext)** of the project which is used to map all the entities over the database connection.

- All the HTTP Methods have proper logging within them which is done using the ILogger Interface.

- There are validations set up in the methods with proper logging along with try catch blocks to handle incorrect inputs.

- Stats like records fetched and time taken to fetch the records are also logged.

To connect to the Cloud instance of the DB I have added the ConnectionString in the appsettings.json class. 

Have added a schema **CapitalFloat** to the DB-

![image](https://user-images.githubusercontent.com/29959387/132464161-2939c114-7114-4730-941e-cc1046f4ef16.png)

Below are some screenshots of the Swagger page when you run the project-

![image](https://user-images.githubusercontent.com/29959387/132460241-adeb405a-20c7-4a7f-b75b-74fb1c3add0e.png)

![image](https://user-images.githubusercontent.com/29959387/132460806-239d9ec3-f6f7-4ad3-8b48-52ced55bfb8b.png)

## GET Method
![image](https://user-images.githubusercontent.com/29959387/132460903-f16ed470-26b7-44d2-a1a4-656a4ce4b648.png)

![image](https://user-images.githubusercontent.com/29959387/132460998-d302e1a6-9c36-452f-9c0b-45a8d5e8469b.png)

## PUT Method
![image](https://user-images.githubusercontent.com/29959387/132461755-6cc6146f-1e43-43f9-bc7b-12b10fe0ccfd.png)

### Output
![image](https://user-images.githubusercontent.com/29959387/132461947-d1f2f08d-f09a-474c-9d51-8fdc116f0c94.png)

![image](https://user-images.githubusercontent.com/29959387/132461895-afea7f71-ed5a-49a6-8583-56dd6c40cb6d.png)

## POST Method
![image](https://user-images.githubusercontent.com/29959387/132462113-e5fc3fd6-7dc0-47f9-843e-4f23e655d251.png)

## DELETE MEthod
![image](https://user-images.githubusercontent.com/29959387/132462251-72f61b9c-9839-4656-bc6e-29c23361eb5a.png)

