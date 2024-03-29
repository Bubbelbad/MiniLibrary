
# MiniLibrary

This is a small version of a library. The goal of this project is to practice MySQL and database connection to the C# application. Also to give different privileges and growing my understanding on views, procedures etc. 


## What I've learned


My first thought was that this would be a test program, so I did not bother to structure the program or UserControls - I just went along. In hindsight planning would've been convenient since MainWindow became too long and the application is not very well structured or following SOLID principles. This makes it a nightmare to maintain and develop.

It has been satisfying to grasp linked tables, PK/FK and the benefit of using MySQL to store data. It's my first project with MySQL and DatabaseConnection so I've got much more to learn, but I've been having a lot of fun with it!
It was great to reverse engineer the script to compare the model to my pre-made EER diagram. It had a few differences that I had to correct.

The hardest part was to figure out a good way of displaying the data from the database in a dynamic and convenient way.


## Features

- WPF Graphic interface with two menus (categorys and functions)
- Function to search for books in the database
- Normal users with limited grants from SQL and admin user with CRUD privileges
- When you borrow a book, a borrow period is created along with a notification.
  When you return the book in time, the borrow period is put to history and notification deleted (I still have to code the part that sends the message at sprcific DateTime though)

There are some functions that I ditn't implement due to lack of time and prioritizing other project, like deleting and updating customers.

## Screenshots
Screenshot from book-view:

![MiniLibrary Screenshot](https://github.com/Bubbelbad/MiniLibrary/blob/master/Screenshot%20-%20Book%20view.png)

Screenshot from EER Diagram:

![EER Diagram Screenshot](https://github.com/Bubbelbad/MiniLibrary/blob/master/Screenshot%20-%20EER%20Diagram.png)
