CREATE TABLE IF NOT EXISTS authentication_users
(
    id int unsigned PRIMARY KEY AUTO_INCREMENT,
    user_name varchar(50) NOT NULL,
    password_hash BINARY(16) NOT NULL,
    password_salt BINARY(16) NOT NULL,
    date_created TIMESTAMP NOT NULL default CURRENT_TIMESTAMP, -- Remove OnUpdate from default Timestamp Behaviour.
    date_last_edited TIMESTAMP on update CURRENT_TIMESTAMP     -- Remove initial default from Timestamp when record is first created.
);

CREATE TABLE IF NOT EXISTS authentication_users_info
(
    id int unsigned PRIMARY KEY AUTO_INCREMENT,
    authentication_user_id int unsigned NOT NULL,
    email varchar(255) NOT NULL,
    date_created TIMESTAMP NOT NULL default CURRENT_TIMESTAMP, -- Remove OnUpdate from default Timestamp Behaviour.
    date_last_edited TIMESTAMP on update CURRENT_TIMESTAMP,     -- Remove initial default from Timestamp when record is first created.
    FOREIGN KEY (authentication_user_id) REFERENCES authentication_users(id)
        ON DELETE CASCADE ON UPDATE RESTRICT
);
