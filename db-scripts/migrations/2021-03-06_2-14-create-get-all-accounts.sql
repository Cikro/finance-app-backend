DELIMITER |
CREATE PROCEDURE
    GetAllAccountsByUserId(userId int UNSIGNED)
    READS SQL DATA
    BEGIN
        SELECT * FROM accounts WHERE user_id = userId;
    END|
DELIMITER ;