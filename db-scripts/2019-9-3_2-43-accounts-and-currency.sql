/* See https://www2.1010data.com/documentationcenter/prime/1010dataUsersGuide/DataTypesAndFormats/currencyUnitCodes.html */
CREATE TABLE IF NOT EXISTS currency 
(
    id int PRIMARY KEY AUTO_INCREMENT,
    entity varchar(255) NOT NULL, -- i.e country, republic, union etc.
    name varchar(255) NOT NULL,
    code char(3) NOT NULL,
    date_created TIMESTAMP NOT NULL default CURRENT_TIMESTAMP, -- Remove OnUpdate from default Timestamp Behaviour.
    date_last_edited TIMESTAMP on update CURRENT_TIMESTAMP     -- Remove initial default from Timestamp when record is first created.
);

CREATE TABLE IF NOT EXISTS account_types 
(
    id int PRIMARY KEY AUTO_INCREMENT,
    name varchar(50) NOT NULL,
    date_created TIMESTAMP NOT NULL default CURRENT_TIMESTAMP,
    date_last_edited TIMESTAMP on update CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS accounts 
(
    id int PRIMARY KEY AUTO_INCREMENT,
    user_id int NOT NULL,
    name varchar(50) NOT NULL UNIQUE,
    description varchar(255),
    balance double default 0,
    type int NOT NULL,
    currency_id int NOT NULL,
    parent_account int default NULL,
    date_created TIMESTAMP NOT NULL default CURRENT_TIMESTAMP,
    date_last_edited TIMESTAMP on update CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id)
        ON DELETE CASCADE ON UPDATE RESTRICT,
    FOREIGN KEY (type) REFERENCES account_types(id)
        ON DELETE RESTRICT ON UPDATE RESTRICT,
    FOREIGN KEY (currency_id) REFERENCES currency(id)
        ON DELETE RESTRICT ON UPDATE RESTRICT,
    FOREIGN KEY (parent_account) REFERENCES accounts(id)
        ON DELETE RESTRICT ON UPDATE RESTRICT -- Stop DELETE if the account has children. If you need to delete the account, remove its children first. 
);

/* If an insert sets the account balance, set the balance to zero */
CREATE TRIGGER IF NOT EXISTS setAccountBalanceZero
    BEFORE INSERT ON accounts
    FOR EACH ROW
    SET NEW.balance = 0;
