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
    created DATETIME,
    notification_type VARCHAR(45),
    borrow_period_id INT,
    FOREIGN KEY (borrow_period_id) REFERENCES borrow_period(id)
);


INSERT INTO customer VALUES (DEFAULT, "admin", "adminsson", "admin", "admin", "approved", true),
							(DEFAULT, "user", "Svensson", "user", "user", "approved", false),
							(DEFAULT, "Bengan", "Bentsson", "bengan.bengtsson@example.se", "1234", "approved", false),
							(DEFAULT, "Farbror", "Barbro", "farbror.barbro@hifiklubben.se", "1234", "denied", false);
							
                            
                            
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
						(DEFAULT, "The Giant Book of Intermediate Classical Piano Music", "G. Schirmer", true);

    
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
		-- INSERT INTO customer_has_book VALUES(book_id, customer_id);
        INSERT INTO borrow_period VALUES(DEFAULT, 
										NOW(), 
                                        DATE_ADD(NOW(), INTERVAL 30 DAY), 
                                        FALSE, 
                                        book_id, 
                                        customer_id);
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
	the_customer_id INT
    )
    BEGIN 
		-- DELETE FROM customer_has_book WHERE book_id = the_book_id && customer_id = the_customer_id;
        UPDATE borrow_period
        SET borrow_period.is_returned = true
        WHERE book_id = the_book_id && customer_id = the_customer_id; 
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
