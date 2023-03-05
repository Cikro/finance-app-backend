CREATE TABLE IF NOT EXISTS application_accounts
(
    id int unsigned PRIMARY KEY AUTO_INCREMENT,
    date_created TIMESTAMP NOT NULL default CURRENT_TIMESTAMP, -- Remove OnUpdate from default Timestamp Behaviour.
    date_last_edited TIMESTAMP on update CURRENT_TIMESTAMP     -- Remove initial default from Timestamp when record is first created.
);


CREATE TABLE IF NOT EXISTS application_users
(
    id int unsigned PRIMARY KEY AUTO_INCREMENT,
    application_account_id int unsigned NOT NULL,
    authentication_user_id int unsigned NOT NULL,  -- basically a forign key for authentication_users. Not sure if it should be defined that way though
    date_created TIMESTAMP NOT NULL default CURRENT_TIMESTAMP, -- Remove OnUpdate from default Timestamp Behaviour.
    date_last_edited TIMESTAMP on update CURRENT_TIMESTAMP,     -- Remove initial default from Timestamp when record is first created.
    FOREIGN KEY (application_account_id) REFERENCES application_accounts(id)
        ON DELETE CASCADE ON UPDATE RESTRICT
);

CREATE TABLE IF NOT EXISTS application_roles
(
    id int unsigned PRIMARY KEY AUTO_INCREMENT,
    role_name varchar(50) NOT NULL,
    date_created TIMESTAMP NOT NULL default CURRENT_TIMESTAMP, -- Remove OnUpdate from default Timestamp Behaviour.
    date_last_edited TIMESTAMP on update CURRENT_TIMESTAMP     -- Remove initial default from Timestamp when record is first created.
);

CREATE TABLE IF NOT EXISTS application_user_roles
(
    id int unsigned PRIMARY KEY AUTO_INCREMENT,
    application_user_id int unsigned NOT NULL,
    role_id varchar(50) NOT NULL,
    date_created TIMESTAMP NOT NULL default CURRENT_TIMESTAMP, -- Remove OnUpdate from default Timestamp Behaviour.
    date_last_edited TIMESTAMP on update CURRENT_TIMESTAMP,     -- Remove initial default from Timestamp when record is first created.
    FOREIGN KEY (application_user_id) REFERENCES application_users(id)
        ON DELETE CASCADE ON UPDATE RESTRICT,
    FOREIGN KEY (role_id) REFERENCES application_roles(id)
        ON DELETE CASCADE ON UPDATE RESTRICT
);

