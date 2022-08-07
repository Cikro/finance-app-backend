DELIMITER |
DROP PROCEDURE IF EXISTS GetAccountsByUserId|
CREATE PROCEDURE
    GetAccountsByUserId(IN userId int UNSIGNED, IN itemsPerPage int unsigned, IN pageOffset int unsigned, OUT totalItems int unsigned)
    READS SQL DATA
    BEGIN
        DECLARE offset int unsigned;

        IF userId <= 0 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'UserId Must be greater than 0.';
        END IF; 
        IF itemsPerPage <= 0 THEN
            SET itemsPerPage = 25;
        END IF; 
        IF pageOffset < 0 THEN
            SET pageOffset = 0;
        END IF; 

        SET offset = pageOffset*itemsPerPage;
        
        SELECT COUNT(id) from accounts INTO totalItems;

        SELECT * FROM accounts WHERE user_id = userId GROUP BY NAME ASC, id ASC LIMIT offset,itemsPerPage;
    END|
DELIMITER ;

SELECT COUNT(id) FROM accounts 