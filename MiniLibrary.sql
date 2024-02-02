DROP DATABASE IF EXISTS MiniLibrary;
CREATE DATABASE MiniLibrary;
USE MiniLibrary;

DROP USER IF EXISTS 'admin'@'localhost';
CREATE USER 'admin'@'localhost' IDENTIFIED BY 'admin';
GRANT SELECT, UPDATE, DELETE, INSERT, EXECUTE
ON minilibrary.*
TO 'admin'@'localhost';


DROP USER IF EXISTS 'user'@'localhost';
CREATE USER 'user'@'localhost' IDENTIFIED BY 'password';
GRANT SELECT, UPDATE
ON minilibrary.*
TO 'user'@'localhost';


DROP TABLE IF EXISTS book;
CREATE TABLE book (
	id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    title VARCHAR(60),
    author VARCHAR(45),
    available BOOLEAN
);
CREATE INDEX book_idx
ON book(title, author);

DROP TABLE IF EXISTS customer;
CREATE TABLE customer (
	id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    first_name VARCHAR(45),
    last_name VARCHAR(45),
    email VARCHAR(320),
    customer_password VARCHAR(45),
    state VARCHAR (45),
    admin BOOLEAN
);

DROP TABLE IF EXISTS borrow_period;
CREATE TABLE borrow_period (
	id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    start_time DATETIME,
    end_time DATETIME,
    is_returned BOOL,
    book_id INT,
    customer_id INT,
    FOREIGN KEY (book_id) REFERENCES book(id),
    FOREIGN KEY (customer_id) REFERENCES customer(id)
);

DROP TABLE IF EXISTS notification;
CREATE TABLE notification (
	id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    send_date DATETIME,
    notification_type VARCHAR(45),
    borrow_period_id INT,
    FOREIGN KEY (borrow_period_id) REFERENCES borrow_period(id)
);


INSERT INTO customer VALUES (DEFAULT, "admin", "adminsson", "admin", "admin", "approved", true),
							(DEFAULT, "user", "Svensson", "user", "user", "approved", false),
							(DEFAULT, "Bengan", "Bengtsson", "bengan.bengtsson@example.se", "1234", "approved", false),
							(DEFAULT, "Farbror", "Barbro", "farbror.barbro@hifiklubben.se", "1234", "denied", false),
                            (DEFAULT, "Al B.", "Bachman", "al.bach.bachman@example.com", "1234", "approved", false),
                            (DEFAULT, "Candice B.", "DePlace", "candice.b.deplace@example.com", "1234", "approved", false),
                            (DEFAULT, "Drew", "Peacock", "drew.peacock@example.com", "1234", "approved", false),
                            (DEFAULT, "Ella", "Vader", "ella.vader@example.com", "1234", "approved", false),
                            (DEFAULT, "Frank N.", "Stein", "frank.n.stein@example.com", "1234", "approved", false),
                            (DEFAULT, "Ginger", "Vitis", "ginger.vitis@example.com", "1234", "approved", false),
                            (DEFAULT, "Ivana", "Tinkle", "ivana.tinkle@example.com", "1234", "approved", false),
                            (DEFAULT, "Jack", "Pott", "jack.pott@example.com", "1234", "approved", false),
                            (DEFAULT, "Justin", "Thyme", "justin.thyme@example.com", "1234", "approved", false),
                            (DEFAULT, "Kurt N.", "Rodd", "kurt.n.rodd@example.com", "1234", "approved", false),
                            (DEFAULT, "Manny", "Kinn", "manny.kinn@example.com", "1234", "approved", false),
                            (DEFAULT, "May B.", "Knott", "may.b.knott@example.com", "1234", "approved", false),
                            (DEFAULT, "Olive", "Yew", "olive.yew@example.com", "1234", "denied", false),
                            (DEFAULT, "Paige", "Turner", "paige.turner@example.com", "1234", "approved", false),
                            (DEFAULT, "Phil", "Harmonic", "phil.harmonic@example.com", "1234", "approved", false),
                            (DEFAULT, "Ray N.", "Carnation", "ray.n.carnation.@example.com", "1234", "approved", false);
                            
							
                            
                            
INSERT INTO book VALUES (DEFAULT, "Weapons of math destruction", "Cathy O'Neil", true),
						(DEFAULT, "How to Change Your Mind", "Michael Pollan", true),
						(DEFAULT, "Odlarens handbok om medicinalväxter", "Simmonds, Monique S. J ", true),
						(DEFAULT, "Prometheus Rising", "Robert Anton Wilson", true),
						(DEFAULT, "Kirke", "Madeline Miller", true),
						(DEFAULT, "Tankar för dagen, manual för ett snällare liv", "Bob Hansson", true),
						(DEFAULT, "King Kong Theory", "Verginie Despentes", true),
						(DEFAULT, "Clean Code", "Robert Cecil Martin", true),
						(DEFAULT, "7 Habits of Highly Effective People", "Sean Covey", true),
						(DEFAULT, "Snus!", "Mats Jonson", true),
						(DEFAULT, "Mr. Norell & Jonathan Strange", "Susanna Clarke", true),
						(DEFAULT, "Konsten att Läsa Tankar", "Henrik Fexeus", true),
						(DEFAULT, "The Giant Book of Intermediate Classical Piano Music", "G. Schirmer", true),
                        (DEFAULT, "The Hitchhiker's Guide to the Galaxy", "Douglas Adams", true),
                        (DEFAULT, "The Catcher in the Rye", "J.D. Salinger", true),
                        (DEFAULT, "The Great Gatsby", "F. Scott Fitzgerald", true),
                        (DEFAULT, "To Kill a Mockingbird", "Harper Lee", true),
                        (DEFAULT, "1984", "George Orwell", true),
                        (DEFAULT, "The Lord of the Rings", "J.R.R. Tolkien", true),
                        (DEFAULT, "The Hobbit", "J.R.R. Tolkien", true),
                        (DEFAULT, "The Chronicles of Narnia", "C.S. Lewis", true),
                        (DEFAULT, "The Da Vinci Code", "Dan Brown", true),
                        (DEFAULT, "The Girl with the Dragon Tattoo", "Stieg Larsson", true);

    
 -- Create new customer   
DELIMITER $$
CREATE PROCEDURE create_new_customer(
    first_name VARCHAR(45),
    last_name VARCHAR(45),
    email VARCHAR(320),
    state VARCHAR (45)
)
BEGIN 	
	INSERT INTO customer VALUES (id, first_name, last_name, email, state);
	SELECT MAX(customer_id) AS "new_id" FROM customer;
END $$
DELIMITER ;


-- Create new book
DELIMITER $$
CREATE PROCEDURE create_new_book(
	title VARCHAR(45),
    author VARCHAR(45),
    available BOOLEAN
    )
    BEGIN
		INSERT INTO book VALUES (id, title, author, available);
        SELECT MAX(id) AS "new_id" FROM book;
	END $$
	DELIMITER ;
    
    
-- Edit existing book
    DELIMITER $$
CREATE PROCEDURE edit_book(
	id INT,
    title VARCHAR(45),
    author VARCHAR(45)
    )
    BEGIN
		UPDATE book
		SET book.title = title, book.author = author -- Här kanske det blir trubbel! 
		WHERE book.id = id; 
	END $$
	DELIMITER ;
    
    
-- Customer borrows book
	DELIMITER $$
CREATE PROCEDURE customer_borrows_book (
	book_id INT,
	customer_id INT
    )
    BEGIN 
        INSERT INTO borrow_period VALUES(DEFAULT, 
										NOW(), 
                                        DATE_ADD(NOW(), INTERVAL 30 DAY), 
                                        FALSE, 
                                        book_id, 
                                        customer_id);
		INSERT INTO notification VALUES(DEFAULT, 
                                        DATE_ADD(NOW(), INTERVAL 30 DAY), 
                                        "Your book is due, please return it",
                                        (SELECT MAX(id) FROM borrow_period));
		UPDATE book 
        SET book.available = false
        WHERE id = book_id;
	END $$
	DELIMITER ;
GRANT EXECUTE ON PROCEDURE minilibrary.customer_borrows_book
TO 'user'@'localhost';

-- Customer returns book
DELIMITER $$
CREATE PROCEDURE customer_returns_book (
	the_book_id INT,
	the_customer_id INT,
    the_borrow_period_id INT
    )
    BEGIN
		DELETE FROM notification WHERE borrow_period_id = the_borrow_period_id;
        UPDATE borrow_period
        SET borrow_period.is_returned = true
        WHERE borrow_period.id = the_borrow_period_id;
        UPDATE borrow_period
        SET borrow_period.end_time = NOW()
        WHERE borrow_period.id = the_borrow_period_id;
		UPDATE book 
        SET book.available = true
        WHERE book.id = the_book_id;
	END $$
	DELIMITER ;
GRANT EXECUTE ON PROCEDURE minilibrary.customer_returns_book
TO 'user'@'localhost';

-- Customer borrowed books
DROP VIEW IF EXISTS customer_borrowed_books;
CREATE VIEW customer_borrowed_books AS
SELECT DISTINCT c.id,
	b.title AS Book,
    bp.start_time AS "Start_Time", bp.end_time AS Deadline, bp.is_returned AS returned
FROM borrow_period bp
INNER JOIN customer c
ON bp.customer_id = c.id
INNER JOIN book b
ON bp.book_id = b.id
WHERE bp.is_returned = false;

SELECT * FROM notification;
