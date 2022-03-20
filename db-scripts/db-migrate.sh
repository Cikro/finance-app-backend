#!/bin/sh

mysql_user="$1"
db_name="$2"

migrationFile="current-migration.txt"

if [[  "$#" -ne 2 ]] 
then 
    echo "	Usage: ./db-migrate [mySQL_user] [db_name]"
    echo "	Output: Runs migrations in name ascending order. Files should be prefixed in the form"
    echo "              yyyy-mm-dd_hh-mm. The most recent migration file will be read last."
    echo "   Note: State is stored in current-migration.txt."
    echo
    exit
fi

# Prevent using a subshell so migrationCount is preserved after while-loop
shopt -s lastpipe

tempFile=$(date)
touch "${tempFile}"
currentMigration=$(cat "${migrationFile}" 2> /dev/null) 

if [[ -z $currentMigration  ]] 
then
    currentMigration=0
fi

migrationCount=0


# Append contents of .db files the occur after the current migration to a tempFile.
ls -1 *.sql | while read file; do

    ((migrationCount++))
    if [[ $migrationCount -gt $currentMigration ]]
    then
        cat $file >> "${tempFile}"
        echo "Migrating.... " $file
    fi
done

mysql -u "${mysql_user}" --database="${db_name}" -p < "${tempFile}"
res=$?
if [[ res -eq 0 ]]
then
    # Append count to state file
    echo $migrationCount> "${migrationFile}"
    echo "Success."

else
    echo "Error occured while migrating..."
    cat "${tempFile}" > errored-sql.txt

fi

rm "${tempFile}" 2> /dev/null
shopt -u lastpipe
