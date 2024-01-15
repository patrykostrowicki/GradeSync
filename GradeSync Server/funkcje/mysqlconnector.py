import mysql.connector

class mysql_connect:
    def connect_to_database():
        try:
            connection = mysql.connector.connect(
                host='localhost',
                user='root',
                password='',
                database='dziennik'
            )
            return connection
        except mysql.connector.Error as e:
            return None