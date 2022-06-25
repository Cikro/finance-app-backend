CREATE TABLE IF NOT EXISTS users 
(
    id int unsigned PRIMARY KEY AUTO_INCREMENT,
    user_name varchar(50) NOT NULL,
    email varchar(255) NOT NULL,
    password_hash varchar(255) NOT NULL,
    date_created TIMESTAMP NOT NULL default CURRENT_TIMESTAMP, -- Remove OnUpdate from default Timestamp Behaviour.
    date_last_edited TIMESTAMP on update CURRENT_TIMESTAMP     -- Remove initial default from Timestamp when record is first created.
);

