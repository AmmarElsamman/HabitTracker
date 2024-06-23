# HabitTracker

Console based CRUD application to track daily habits.
Developed using C# and Postgres.


# Given Requirements:
- [x] It should create 3 tables in the database, for habits to be tracked, units for the measuring of each habit ,and table for records of each habit.
- [x] You need to be able to insert, delete, update and view your logged records. 
- [x] You should handle all possible errors so that the application never crashes. 
- [x] The application should only be terminated when the user inserts 0. 
- [x] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
- [x] User should be able to create their own habits to track, and choose the unit of measurement of each habit.

# Features

* Postgres database connection

	- The program uses a local Postgres db connection to store and read information. 
	- If the correct tables does not exist they will be created on program start.

* A console based UI where users can navigate by key presses
  - ![Screenshot 2024-06-23 123933](https://github.com/AmmarElsamman/HabitTracker/assets/53655392/37374496-52fc-4d43-9034-130c599fea51)

* CRUD DB functions

  - From the main menu users can Create, Read, Update or Delete entries for whichever date they want, entered in dd-MM-yy format.
  - Inserting same habit on the same day will result in only incrementing the quantity. 
	- Time and Dates inputted are checked to make sure they are in the correct and realistic format.

* Basic Reporting of Records

	- ![Screenshot 2024-06-23 124446](https://github.com/AmmarElsamman/HabitTracker/assets/53655392/91f9a25f-5db2-4e3b-890e-353791473e8f)


