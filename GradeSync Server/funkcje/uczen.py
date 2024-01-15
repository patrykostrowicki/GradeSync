import json
from . import mysqlconnector

class zadanie:
    def dane(login):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        #pobieranie danych z tabeli uczen
        query_uczen = "SELECT imie_nazwisko, klasa, ocena_z_zachowania FROM uczniowie WHERE login = %s"
        cursor.execute(query_uczen, (login,))
        result_uczen = cursor.fetchone()

        #pobieranie planów zajęć dla obu semestrów
        plany = {}
        for semestr in [1, 2]:
            query_plan_zajec = """
                SELECT poniedzialek, wtorek, sroda, czwartek, piatek
                FROM Plan_zajec
                WHERE klasa = %s AND semestr = %s
            """
            cursor.execute(query_plan_zajec, (result_uczen[1], semestr))
            plan = cursor.fetchone()
            
            #dekodowanie wartości json
            plany[f'plan{semestr}'] = {
                "poniedzialek": json.loads(plan[0].decode('utf-8')) if plan[0] else {},
                "wtorek": json.loads(plan[1].decode('utf-8')) if plan[1] else {},
                "sroda": json.loads(plan[2].decode('utf-8')) if plan[2] else {},
                "czwartek": json.loads(plan[3].decode('utf-8')) if plan[3] else {},
                "piatek": json.loads(plan[4].decode('utf-8')) if plan[4] else {}
            }

        query_oceny = "SELECT przedmiot, ocena, wystawil, data_wystawienia,opis FROM oceny WHERE uczen_login = %s"
        cursor.execute(query_oceny, (login,))
        oceny = cursor.fetchall()

        query_frekwencja = "SELECT data, przedmiot, typ FROM Frekwencja WHERE login = %s"
        cursor.execute(query_frekwencja, (login,))
        frekwencja = cursor.fetchall()

        frekwencja_list = [
            {
                "data": str(data),
                "przedmiot": przedmiot,
                "typ": typ
            }
            for data, przedmiot, typ in frekwencja
        ]

        oceny_dict = [
            {
                "przedmiot": przedmiot,
                "ocena": ocena,
                "wystawil": wystawil,
                "data_wystawienia": data_wystawienia,
                "opis": opis
            }
            for przedmiot, ocena, wystawil, data_wystawienia, opis in oceny
        ]

        query_uwagi = "SELECT wystawil, data, tresc, typ FROM uwagi_i_osiagniecia WHERE uczen_login = %s"
        cursor.execute(query_uwagi, (login,))
        uwagi = cursor.fetchall()

        uwagi_list = [
            {
                "wystawil": wystawil,
                "data": str(data),
                "tresc": tresc,
                "typ": typ
            }
            for wystawil, data, tresc, typ in uwagi
        ]

        query_wydarzenia = "SELECT wystawil, data, termin, typ, opis, przedmiot FROM wydarzenia WHERE klasa = %s"
        cursor.execute(query_wydarzenia, (result_uczen[1],))
        wydarzenia = cursor.fetchall()

        wydarzenia_list = [
            {
                "wystawil": wystawil,
                "data": str(data),
                "termin": str(termin),
                "typ": typ,
                "opis": opis,
                "przedmiot": przedmiot
            }
            for wystawil, data, termin, typ, opis, przedmiot in wydarzenia
        ]

        cursor.close()
        conn.close()
        
        response_data = {
                "imie_nazwisko": result_uczen[0],
                "klasa": result_uczen[1],
                "ocena_z_zachowania": result_uczen[2],
                "oceny": oceny_dict,
                "plan1": plany['plan1'],
                "plan2": plany['plan2'],
                "frekwencja": frekwencja_list,
                "uwagi": uwagi_list,
                "wydarzenia": wydarzenia_list
            }

        return response_data
    
    def untk(klasa, przedmiot, user_login):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        query_oceny = """
        SELECT ocena, data_wystawienia 
        FROM oceny 
        WHERE klasa = %s AND przedmiot = %s AND uczen_login != %s
        """
        cursor.execute(query_oceny, (klasa, przedmiot, user_login))
        wyniki = cursor.fetchall()

        cursor.close()
        conn.close()

        return [(wynik[0], wynik[1].strftime("%Y-%m-%d")) for wynik in wyniki]

    def zou(klasa, user_login, nauczyciel_login):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        if nauczyciel_login == "wszyscy":
            query_oceny = """
            SELECT ocena, data_wystawienia, przedmiot, opis
            FROM oceny 
            WHERE klasa = %s AND uczen_login = %s
            """
            cursor.execute(query_oceny, (klasa, user_login))
        else:
            query_oceny = """
            SELECT ocena, data_wystawienia, przedmiot, opis
            FROM oceny 
            WHERE klasa = %s AND wystawil_login = %s AND uczen_login = %s
            """
            cursor.execute(query_oceny, (klasa, nauczyciel_login, user_login))

        wyniki = cursor.fetchall()

        query_ocena_zachowania = """
        SELECT ocena_z_zachowania
        FROM uczniowie
        WHERE login = %s
        """
        cursor.execute(query_ocena_zachowania, (user_login,))
        ocena_zachowania = cursor.fetchone()

        cursor.close()
        conn.close()

        if ocena_zachowania:
            wyniki.append(('Ocena z zachowania', None, None, ocena_zachowania[0]))

        return [(wynik[0], wynik[1].strftime("%Y-%m-%d") if wynik[1] else None, wynik[2], wynik[3]) for wynik in wyniki]

    def pobierz_uwagi_i_osiagniecia(login):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()
        query_uwagi = "SELECT wystawil, data, tresc, typ FROM uwagi_i_osiagniecia WHERE uczen_login = %s"
        cursor.execute(query_uwagi, (login,))
        uwagi = cursor.fetchall()

        uwagi_list = [
            {
                "wystawil": wystawil,
                "data": str(data),
                "tresc": tresc,
                "typ": typ
            }
            for wystawil, data, tresc, typ in uwagi
        ]

        cursor.close()
        conn.close()

        return uwagi_list

    def pobierz_frekwencje(login):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        query_frekwencja = "SELECT data, przedmiot, typ FROM Frekwencja WHERE login = %s"
        cursor.execute(query_frekwencja, (login,))
        frekwencja = cursor.fetchall()

        frekwencja_list = [
            {
                "data": str(data),
                "przedmiot": przedmiot,
                "typ": typ
            }
            for data, przedmiot, typ in frekwencja
        ]

        cursor.close()
        conn.close()

        return frekwencja_list
