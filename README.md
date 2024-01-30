
# MiniLibrary

This is a small version of a library. The goal of this project is to practice MySQL and database connection to the C# application. Also to give different privileges and growing my understanding on views, procedures etc. 

My first thought was that this would be a very small test program so I did not bother with usercontrols or separating the code in as many classes (in hindsight that would've been convenient since MainWindow is way too long now).


## What I've learned

It has been satisfying to grasp linked tables, PK/FK and the benefit of using MySQL to store data. It's my first project so I've got much more to learn, but I've been having a lot of fun with it!
Although my ER diagram was pretty good, I missed some details at the end for the borrow_period's FK's. It was great to reverse engineer the diagram to compare it to the one I made before and see that it did differ with that detail.

The hardest part I think is to figure out a good way of displaying the data from the database in a convenient way. I have all the data I need in c#, so how do I combine it from different lists? Do I really need to create classes for specific views? It seems like there has to be a better way. If anyone reading this knows what I mean - please give me a hint. I tried CompositeCollection with two lists as a source, and then wrapping it - but it didnt quite work as I wanted. 

## Features

- Menu to browse books and to use them in different functions.
- Normal users with limited grants from SQL and admin user with full privileges
- Main point - search for books and borrow them. When a book is borrowed a 'borrow period' is initiated with DateTime
- Add, Delete, Update and use Views and functions from SQL. 


## Screenshots

![MiniLibrary Screenshot](https://github.com/Bubbelbad/MiniLibrary/blob/master/Screenshot%20MiniLibrary.png)
