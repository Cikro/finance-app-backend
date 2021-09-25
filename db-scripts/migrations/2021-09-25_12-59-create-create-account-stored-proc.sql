DELIMITER |
DROP PROCEDURE IF EXISTS CreateAccount|
CREATE PROCEDURE
    CreateAccount(IN userId int UNSIGNED, IN accountName varchar(255), IN accountDescription varchar(255), IN accountType tinyint unsigned, In currencyCode CHAR(3), IN parentAccountId int UNSIGNED)
    READS SQL DATA
    BEGIN
        DECLARE offset int unsigned;

        IF userId <= 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'UserId Must be greater than 0.';
        END IF; 
        IF parentAccountId <= 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'parentAccountId Must be greater than 0.';
        END IF; 

        SET offset = pageOffset*itemsPerPage;
    
        INSERT INTO accounts (user_id, name, description, type, currency_code, parent_account) VALUES (userId, accountName, accountDescription, accountType, currencyCode, parentAccountId);
        SELECT * FROM accounts WHERE id=(SELECT LAST_INSERT_ID());
    END|
DELIMITER ;