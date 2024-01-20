import json

from flask import jsonify
from . import mysqlconnector
from datetime import datetime

class zadanie:
    def dane(login):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        try:
            cursor = conn.cursor()

            query_uczniowie = "SELECT login, imie_nazwisko, klasa FROM uczniowie"
            cursor.execute(query_uczniowie)
            result_uczniowie = cursor.fetchall()

            query_nauczyciele = "SELECT login, imie_nazwisko, klasa, inne FROM nauczyciele"
            cursor.execute(query_nauczyciele)
            result_nauczyciele = cursor.fetchall()

            result_nauczyciele_processed = []
            for record in result_nauczyciele:
                processed_record = list(record)
                result_nauczyciele_processed.append(processed_record)

            query_admini = "SELECT login FROM uzytkownicy where typ = 'admin'"
            cursor.execute(query_admini)
            result_admini = cursor.fetchall()

            #ustalanie obecnego semestru
            dzisiaj = datetime.now()
            rok = dzisiaj.year
            semestr = 2 if datetime(rok, 1, 1) <= dzisiaj <= datetime(rok, 6, 21) else 1

            #pobieranie planów zajęć dla obecnego semestru
            query_plan_zajec = "SELECT klasa, poniedzialek, wtorek, sroda, czwartek, piatek FROM plan_zajec WHERE semestr = %s"
            cursor.execute(query_plan_zajec, (semestr,))
            plans = cursor.fetchall()

            plany = []

            for plan in plans:
                plany.append({
                    "klasa": plan[0],
                    "poniedzialek": json.loads(plan[1]) if plan[1] else {},
                    "wtorek": json.loads(plan[2]) if plan[2] else {},
                    "sroda": json.loads(plan[3]) if plan[3] else {},
                    "czwartek": json.loads(plan[4]) if plan[4] else {},
                    "piatek": json.loads(plan[5]) if plan[5] else {}
                })


            response_data = {
                "uczniowie": result_uczniowie,
                "nauczyciele": result_nauczyciele_processed,
                "admini": result_admini,
                "plany_lekcji": plany
            }

            return response_data
        except Exception as error:
            print(f"Wystąpił problem: {error}")
            return False
        finally:
            cursor.close()
            conn.close()
            
    def usun_konto(login):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        try:
            cursor = conn.cursor()
            
            query_typ_uzytkownika = "SELECT typ FROM uzytkownicy WHERE login = %s"
            cursor.execute(query_typ_uzytkownika, (login,))
            typ_uzytkownika = cursor.fetchone()

            if typ_uzytkownika[0] == 'uczen':
                query_usun_uczniowie = "DELETE FROM uczniowie WHERE login = %s"
                cursor.execute(query_usun_uczniowie, (login,))
            elif typ_uzytkownika[0] == 'nauczyciel':
                query_usun_nauczyciele = "DELETE FROM nauczyciele WHERE login = %s"
                cursor.execute(query_usun_nauczyciele, (login,))

            query_usun_uzytkownicy = "DELETE FROM uzytkownicy WHERE login = %s"
            cursor.execute(query_usun_uzytkownicy, (login,))

            conn.commit()
            return True

        except Exception as e:
            conn.rollback()
            print(f"Wystąpił błąd: {e}")
            return False

        finally:
            cursor.close()
            conn.close()


    def utworz_ucznia(login, imie_nazwisko, haslo, klasa, ocena_z_zachowania):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        try:
            cursor = conn.cursor()

            query_dodaj_uzytkownicy = """
            INSERT INTO uzytkownicy (login, haslo, typ) 
            VALUES (%s, %s, 'uczen')
            """
            cursor.execute(query_dodaj_uzytkownicy, (login, haslo))

            query_dodaj_uczniowie = """
            INSERT INTO uczniowie (login, imie_nazwisko, klasa, ocena_z_zachowania) 
            VALUES (%s, %s, %s, %s)
            """
            cursor.execute(query_dodaj_uczniowie, (login, imie_nazwisko, klasa, ocena_z_zachowania))

            conn.commit()
            return True

        except Exception as e:
            conn.rollback()
            print(f"Wystąpił błąd: {e}")
            return False

        finally:
            cursor.close()
            conn.close()

    def aktualizuj_ucznia(login, imie_nazwisko, klasa):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        try:
            cursor = conn.cursor()
            query = "UPDATE uczniowie SET imie_nazwisko = %s, klasa = %s WHERE login = %s"
            cursor.execute(query, (imie_nazwisko, klasa, login))
            conn.commit()
            return True
        except Exception as e:
            print(f"Wystąpił błąd: {e}")
            conn.rollback()
            return False
        finally:
            cursor.close()
            conn.close()

    def utworz_nauczyciela(login, imie_nazwisko, haslo, klasa, przedmioty):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        try:
            cursor = conn.cursor()

            query_dodaj_uzytkownicy = """
            INSERT INTO uzytkownicy (login, haslo, typ) 
            VALUES (%s, %s, 'nauczyciel')
            """
            cursor.execute(query_dodaj_uzytkownicy, (login, haslo))

            query_dodaj_nauczyciele = """
            INSERT INTO nauczyciele (login, imie_nazwisko, klasa, inne) 
            VALUES (%s, %s, %s, %s)
            """
            cursor.execute(query_dodaj_nauczyciele, (login, imie_nazwisko, klasa, przedmioty))

            conn.commit()
            return True

        except Exception as e:
            conn.rollback()
            print(f"Wystąpił błąd: {e}")
            return False

        finally:
            cursor.close()
            conn.close()
    
    def aktualizuj_nauczyciela(login, imie_nazwisko, klasa, przedmioty):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        try:
            cursor = conn.cursor()
            query_nauczyciel = "UPDATE nauczyciele SET imie_nazwisko = %s, klasa = %s, inne = %s WHERE login = %s"
            cursor.execute(query_nauczyciel, (imie_nazwisko, klasa, przedmioty, login))
            conn.commit()
            return True
        except Exception as e:
            print(f"Wystąpił błąd: {e}")
            conn.rollback()
            return False
        finally:
            cursor.close()
            conn.close()

    def utworz_admina(login, haslo):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False
        
        try:
            cursor = conn.cursor()

            query = """
            INSERT INTO uzytkownicy (login, haslo, typ) 
            VALUES (%s, %s, 'admin')
            """
            cursor.execute(query, (login, haslo))
            conn.commit()
            return True

        except Exception as e:
            print(f"Wystąpił błąd: {e}")
            if conn:
                conn.rollback()
            return False

        finally:
            if cursor:
                cursor.close()
            if conn:
                conn.close()

    def usun_plan_zajec(klasa, semestr):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        try:
            cursor = conn.cursor()

            query_usun_plan = "DELETE FROM plan_zajec WHERE klasa = %s AND semestr = %s"
            cursor.execute(query_usun_plan, (klasa, semestr))

            conn.commit()
            return True

        except Exception as e:
            conn.rollback()
            print(f"Wystąpił błąd: {e}")
            return False

        finally:
            cursor.close()
            conn.close()

    def dodaj_plan_lekcji(klasa, semestr, poniedzialek, wtorek, sroda, czwartek, piatek):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False
        
        try:
            cursor = conn.cursor()

            query = """
            INSERT INTO plan_zajec (klasa, semestr, poniedzialek, wtorek, sroda, czwartek, piatek) 
            VALUES (%s, %s, %s, %s, %s, %s, %s)
            """
            cursor.execute(query, (klasa, semestr, poniedzialek, wtorek, sroda, czwartek, piatek))
            conn.commit()
            return True

        except Exception as e:
            print(f"Wystąpił błąd: {e}")
            if conn:
                conn.rollback()
            return False

        finally:
            if cursor:
                cursor.close()
            if conn:
                conn.close()