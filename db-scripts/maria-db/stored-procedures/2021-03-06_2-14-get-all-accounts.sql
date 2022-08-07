DELIMITER |
DROP PROCEDURE IF EXISTS GetAllAccountsByUserId|
CREATE PROCEDURE
    GetAllAccountsByUserId(userId int UNSIGNED)
    READS SQL DATA
    BEGIN
        SELECT * FROM accounts WHERE user_id = userId GROUP BY NAME ASC, date_created ASC;
    END|
DELIMITER ;