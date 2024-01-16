from datetime import datetime
import json
from . import mysqlconnector

class zadanie:
    @staticmethod
    def dane(login):
        try:
            conn = mysqlconnector.mysql_connect.connect_to_database()
            if conn is None:
                print("Nie udało się połączyć z bazą danych.")
                return False

            cursor = conn.cursor(buffered=True)

            query_nauczyciel = "SELECT imie_nazwisko, klasa, inne FROM nauczyciele WHERE login = %s"
            cursor.execute(query_nauczyciel, (login,))
            result_nauczyciel = cursor.fetchone()

            query_uczniowie = "SELECT login, imie_nazwisko, klasa FROM uczniowie"
            cursor.execute(query_uczniowie)
            result_uczniowie = cursor.fetchall()
            uczniowie_list = [{"login": uczen[0], "imie_nazwisko": uczen[1], "klasa": uczen[2]} for uczen in result_uczniowie]

            query_wydarzenia = "SELECT klasa, wystawil, wystawil_login, data, termin, przedmiot, opis, typ FROM wydarzenia"
            cursor.execute(query_wydarzenia)
            result_wydarzenia = cursor.fetchall()
            wydarzenia_list = [{"klasa": wydarzenie[0], "wystawil": wydarzenie[1], "wystawil_login": wydarzenie[2], "data": wydarzenie[3], "termin": wydarzenie[4], "przedmiot": wydarzenie[5], "opis": wydarzenie[6], "typ": wydarzenie[7]} for wydarzenie in result_wydarzenia]

            query_uwagi = "SELECT uczen, uczen_login, data, tresc, klasa, typ FROM uwagi_i_osiagniecia WHERE wystawil_login = %s"
            cursor.execute(query_uwagi, (login,))
            result_uwagi = cursor.fetchall()
            uwagi_list = [{"uczen": uwaga[0], "uczen_login": uwaga[1], "data": uwaga[2], "tresc": uwaga[3], "klasa": uwaga[4], "typ": uwaga[5]} for uwaga in result_uwagi]

            dzisiaj = datetime.now()
            rok = dzisiaj.year
            semestr = None
            if datetime(rok, 1, 1) <= dzisiaj <= datetime(rok, 6, 21):
                semestr = 2
            elif datetime(rok, 9, 1) <= dzisiaj <= datetime(rok, 12, 15):
                semestr = 1
            elif datetime(rok - 1, 12, 16) <= dzisiaj < datetime(rok, 1, 1):
                semestr = 2

            today = dzisiaj.strftime("%A")
            days_mapping = {"Monday": "poniedzialek", "Tuesday": "wtorek", "Wednesday": "sroda", "Thursday": "czwartek", "Friday": "piatek"}
            today_pl = days_mapping.get(today, "poniedzialek")

            query_plan_zajec = f"SELECT klasa, {today_pl} FROM plan_zajec WHERE semestr = %s"
            cursor.execute(query_plan_zajec, (semestr,))
            result_plan_zajec = cursor.fetchall()

            zajecia = []
            for klasa, plan_dnia in result_plan_zajec:
                plan_dnia = json.loads(plan_dnia)
                for lekcja, szczegoly in plan_dnia.items():
                    if szczegoly.get('prowadzacy_login') == login:
                        zajecia.append({
                            "klasa": klasa,
                            "przedmiot": szczegoly.get('przedmiot')
                        })

        except Exception as e:
            print(f"Wystąpił błąd: {e}")
            return False
        finally:
            cursor.close()
            conn.close()

        response_data = {
            "imie_nazwisko": result_nauczyciel[0],
            "klasa":result_nauczyciel[1],
            "inne": json.loads(result_nauczyciel[2]) if result_nauczyciel[2] else {},
            "uczniowie": uczniowie_list,
            "wydarzenia": wydarzenia_list,
            "uwagi_i_osiagniecia": uwagi_list,
            "zajecia": zajecia
        }
    
        return response_data



    def usun_ocene(opis, ocena, klasa, uczen_login, wystawil_login, przedmiot, data_wystawienia_str):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        try:
            data_wystawienia = datetime.strptime(data_wystawienia_str, '%Y-%m-%d').date()
            delete_query = """
            DELETE FROM oceny 
            WHERE uczen_login = %s AND przedmiot = %s AND data_wystawienia = %s AND klasa = %s AND wystawil_login = %s AND ocena = %s AND opis = %s
            """
            delete_values = (uczen_login, przedmiot, data_wystawienia, klasa, wystawil_login, ocena, opis)

            cursor.execute(delete_query, delete_values)
            conn.commit()

            if cursor.rowcount > 0:
                return True
            else:
                return False
        except Exception as error:
            print(f"Wystąpił problem przy usuwaniu oceny: {error}")
            return False
        finally:
            cursor.close()
            conn.close()

    def edytuj_ocene(opis, ocena, klasa, uczen_login, wystawil_login, przedmiot, data_wystawienia_str, nowa_ocena):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        try:
            data_wystawienia = datetime.strptime(data_wystawienia_str, '%Y-%m-%d').date()
            update_query = """
            UPDATE oceny 
            SET ocena = %s
            WHERE uczen_login = %s AND przedmiot = %s AND data_wystawienia = %s AND klasa = %s AND wystawil_login = %s AND ocena = %s AND opis = %s
            """
            update_values = (nowa_ocena, uczen_login, przedmiot, data_wystawienia, klasa, wystawil_login, ocena, opis)

            cursor.execute(update_query, update_values)
            conn.commit()

            if cursor.rowcount > 0:
                return True
            else:
                return False
        except Exception as error:
            print(f"Wystąpił problem przy edytowaniu oceny: {error}")
            return False
        finally:
            cursor.close()
            conn.close()

    def wystaw_ocene(uczen_login, wystawil, ocena, przedmiot, klasa, opis, wystawil_login):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        try:
            if przedmiot == "Zachowanie":
                update_query = """
                UPDATE uczniowie
                SET ocena_z_zachowania = %s
                WHERE login = %s
                """
                update_values = (ocena, uczen_login)
                cursor.execute(update_query, update_values)
            else:
                insert_query = """
                INSERT INTO oceny (uczen_login, wystawil, ocena, przedmiot, klasa, opis, wystawil_login) 
                VALUES (%s, %s, %s, %s, %s, %s, %s)
                """
                insert_values = (uczen_login, wystawil, ocena, przedmiot, klasa, opis, wystawil_login)
                cursor.execute(insert_query, insert_values)

            conn.commit()
            return True
        except Exception as error:
            print(f"Wystąpił problem: {error}")
            return False
        finally:
            cursor.close()
            conn.close()

    def usun_wydarzenie(nauczyciel_login, data_str, termin_str, przedmiot, opis, typ, klasa):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        try:
            data = datetime.strptime(data_str, '%Y-%m-%d').date()
            termin = datetime.strptime(termin_str, '%Y-%m-%d').date() if termin_str else None

            delete_query = """
            DELETE FROM wydarzenia 
            WHERE wystawil_login = %s AND klasa = %s AND data = %s AND termin = %s AND przedmiot = %s AND opis = %s AND typ = %s
            """
            delete_values = (nauczyciel_login, klasa, data, termin, przedmiot, opis, typ)

            cursor.execute(delete_query, delete_values)
            conn.commit()

            if cursor.rowcount > 0:
                return True
            else:
                return False
        except Exception as error:
            print(f"Wystąpił problem przy usuwaniu wydarzenia: {error}")
            return False
        finally:
            cursor.close()
            conn.close()

    def edytuj_wydarzenie(nauczyciel_login, data_str, termin_str, przedmiot, opis, typ, klasa, nowy_termin_str, nowy_opis, nowy_typ):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        try:
            data = datetime.strptime(data_str, '%Y-%m-%d').date()
            termin = datetime.strptime(termin_str, '%Y-%m-%d').date() if termin_str else None
            nowy_termin = datetime.strptime(nowy_termin_str, '%Y-%m-%d').date() if nowy_termin_str else None

            update_query = """
            UPDATE wydarzenia
            SET termin = %s, opis = %s, typ = %s
            WHERE wystawil_login = %s AND klasa = %s AND data = %s AND termin = %s AND przedmiot = %s AND opis = %s AND typ = %s
            """
            update_values = (nowy_termin, nowy_opis, nowy_typ, nauczyciel_login, klasa, data, termin, przedmiot, opis, typ)

            cursor.execute(update_query, update_values)
            conn.commit()

            if cursor.rowcount > 0:
                return True
            else:
                return False
        except Exception as error:
            print(f"Wystąpił problem przy edytowaniu wydarzenia: {error}")
            return False
        finally:
            cursor.close()
            conn.close()

    def utworz_wydarzenie(nauczyciel_login, nauczyciel_in, termin_str, przedmiot, opis, typ, klasa):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        try:
            termin = datetime.strptime(termin_str, '%Y-%m-%d').date() if termin_str else None

            insert_query = """
            INSERT INTO wydarzenia (klasa, wystawil, wystawil_login, data, termin, przedmiot, opis, typ)
            VALUES (%s, %s, %s, %s, %s, %s, %s, %s)
            """
            current_date = datetime.now().date()
            insert_values = (klasa, nauczyciel_in, nauczyciel_login, current_date, termin, przedmiot, opis, typ)

            cursor.execute(insert_query, insert_values)
            conn.commit()

            if cursor.rowcount > 0:
                return True
            else:
                return False
        except Exception as error:
            print(f"Wystąpił problem przy tworzeniu wydarzenia: {error}")
            return False
        finally:
            cursor.close()
            conn.close()

    def usun_uwage(nauczyciel_login, uczen_login, data_str, tresc, typ, klasa):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        try:
            data = datetime.strptime(data_str, '%Y-%m-%d').date() if data_str else None
            typ = int(typ) if typ else None

            delete_query = """
            DELETE FROM uwagi_i_osiagniecia
            WHERE wystawil_login = %s AND uczen_login = %s AND data = %s AND tresc = %s AND typ = %s AND klasa = %s
            """
            delete_values = (nauczyciel_login, uczen_login, data, tresc, typ, klasa)

            cursor.execute(delete_query, delete_values)
            conn.commit()

            if cursor.rowcount > 0:
                return True
            else:
                return False
        except Exception as error:
            print(f"Wystąpił problem przy usuwaniu uwagi: {error}")
            return False
        finally:
            cursor.close()
            conn.close()

    def edytuj_uwage(nauczyciel_login, uczen_login, data_str, tresc, typ, klasa, nowa_tresc, nowy_typ):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        try:
            data = datetime.strptime(data_str, '%Y-%m-%d').date() if data_str else None
            typ = int(typ) if typ else None
            nowy_typ = int(nowy_typ) if nowy_typ else None

            update_query = """
            UPDATE uwagi_i_osiagniecia
            SET tresc = %s, typ = %s
            WHERE wystawil_login = %s AND uczen_login = %s AND data = %s AND tresc = %s AND typ = %s AND klasa = %s
            """
            update_values = (nowa_tresc, nowy_typ, nauczyciel_login, uczen_login, data, tresc, typ, klasa)

            cursor.execute(update_query, update_values)
            conn.commit()

            if cursor.rowcount > 0:
                return True
            else:
                return False
        except Exception as error:
            print(f"Wystąpił problem przy edytowaniu uwagi: {error}")
            return False
        finally:
            cursor.close()
            conn.close()            

    def utworz_uwage(nauczyciel_login, uczen_login, uczen, tresc, typ, klasa, nauczyciel):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        try:
            current_date = datetime.now().date()
            typ = int(typ) if typ else None

            insert_query = """
            INSERT INTO uwagi_i_osiagniecia (uczen, uczen_login, wystawil, wystawil_login, data, tresc, typ, klasa)
            VALUES (%s, %s, %s, %s, %s, %s, %s, %s)
            """

            insert_values = (uczen, uczen_login, nauczyciel, nauczyciel_login, current_date, tresc, typ, klasa)

            cursor.execute(insert_query, insert_values)
            conn.commit()

            if cursor.rowcount > 0:
                return True
            else:
                return False
        except Exception as error:
            print(f"Wystąpił problem przy tworzeniu uwagi: {error}")
            return False
        finally:
            cursor.close()
            conn.close()

    def zapisz_frekwencje(przedmiot, typ, login, uczen, klasa):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        try:
            insert_query = """
            INSERT INTO frekwencja (przedmiot, typ, login, uczen, klasa)
            VALUES (%s, %s, %s, %s, %s)
            """
            insert_values = (przedmiot, int(typ), login, uczen, klasa)

            cursor.execute(insert_query, insert_values)
            conn.commit()

            if cursor.rowcount > 0:
                return True
            else:
                return False
        except Exception as error:
            print(f"Wystąpił problem przy dodawaniu frekwencji: {error}")
            return False
        finally:
            cursor.close()
            conn.close()
    
    def pobierz_frekwencje(login):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        try:
            query = """
            SELECT data, przedmiot, typ, uczen, klasa
            FROM frekwencja
            WHERE login = %s
            """
            cursor.execute(query, (login,))
            rows = cursor.fetchall()

            frekwencja = []
            for row in rows:
                frekwencja.append({
                    "data": row[0].strftime('%Y-%m-%d %H:%M:%S'),
                    "przedmiot": row[1],
                    "typ": row[2]
                })

            return frekwencja
        except Exception as error:
            print(f"Wystąpił problem przy pobieraniu frekwencji: {error}")
            return []
        finally:
            cursor.close()
            conn.close()

    def usun_frekwencje(przedmiot, typ, login, uczen, klasa, data):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        try:
            delete_query = """
            DELETE FROM frekwencja
            WHERE przedmiot = %s AND typ = %s AND login = %s AND uczen = %s AND klasa = %s AND data = %s
            """
            delete_values = (przedmiot, typ, login, uczen, klasa, data)

            cursor.execute(delete_query, delete_values)
            conn.commit()

            return cursor.rowcount > 0
        except Exception as error:
            print(f"Wystąpił problem przy usuwaniu frekwencji: {error}")
            return False
        finally:
            cursor.close()
            conn.close()

    def edytuj_frekwencje(przedmiot, typ, nowy_typ, login, uczen, klasa, data):
        conn = mysqlconnector.mysql_connect.connect_to_database()
        if conn is None:
            print("Nie udało się połączyć z bazą danych.")
            return False

        cursor = conn.cursor()

        try:
            update_query = """
            UPDATE frekwencja
            SET typ = %s
            WHERE przedmiot = %s AND typ = %s AND login = %s AND uczen = %s AND klasa = %s AND data = %s
            """
            update_values = (nowy_typ, przedmiot, typ, login, uczen, klasa, data)

            cursor.execute(update_query, update_values)
            conn.commit()

            return cursor.rowcount > 0
        except Exception as error:
            print(f"Wystąpił problem przy edytowaniu frekwencji: {error}")
            return False
        finally:
            cursor.close()
            conn.close()