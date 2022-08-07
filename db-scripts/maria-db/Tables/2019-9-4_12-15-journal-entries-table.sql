CREATE TABLE IF NOT EXISTS journal_entries 
(
    id int unsigned PRIMARY KEY AUTO_INCREMENT,
    user_id int unsigned NOT NULL,
    amount DECIMAL(15,2),
    corrected boolean default false,
    server_generated boolean default false,
    date_created TIMESTAMP NOT NULL default CURRENT_TIMESTAMP,
    date_last_edited TIMESTAMP on update CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id)
        ON DELETE CASCADE ON UPDATE RESTRICT
);
