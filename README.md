
# MiniLibrary

This is a small version of a library. The goal of this project is to practice MySQL and database connection to create, read, update and delete (CRUD) within the database from the C# application. 

My first thought was that this would be a small test program so I did not bother with usercontrols or separating the code in as many classes. Now that would be convenient since MainWindow is a bit long. 


## What I've learned

It has been satisfying to grasp linked tables, PK/FK and the benefit of using MySQL to store data. It's my first project so I've got much more to learn, but I've been having a lot of fun with it!
Although my ER diagram was pretty good, I missed some details at the end for the borrow_period's FK's. It was great to reverse engineer the diagram to compare it to the one I made before and see that it did differ with that detail.

## Features

- Menu to browse books and to use them in different functions.
- Normal users with limited grants from SQL and admin user with full privileges
- Main point - search for books and borrow them. When a book is borrowed a 'borrow period' is initiated with DateTime
- Add, Delete, Update and use Views and functions from SQL. 


## Screenshots

![MiniLibrary Screenshot](https://github.com/Bubbelbad/MiniLibrary/blob/master/Screenshot%20MiniLibrary.png)
