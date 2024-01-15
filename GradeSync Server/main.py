from flask import Flask, request, json, Response, jsonify
from funkcje import mysqlconnector, nauczyciel, uczen, admin

app = Flask(__name__)

@app.route('/login', methods=['GET'])
def login():
    user_login = request.args.get('login')
    user_password = request.args.get('haslo')

    conn = mysqlconnector.mysql_connect.connect_to_database()
    if conn is not None and conn.is_connected():
        cursor = conn.cursor()

        query = "SELECT typ FROM uzytkownicy WHERE login = %s AND haslo = %s"
        cursor.execute(query, (user_login, user_password))
        result = cursor.fetchone()
        cursor.close()
        conn.close()

        if result:
            if result[0] == "uczen":
                user_type = result[0]
                response_data = {'type': user_type, 'data': uczen.zadanie.dane(user_login) if user_type == 'uczen' else {}}
                response_json = json.dumps(response_data, ensure_ascii=False)
                return Response(response_json, content_type="application/json; charset=utf-8")
            elif result[0] == "nauczyciel":
                user_type = result[0]
                response_data = {'type': user_type, 'data': nauczyciel.zadanie.dane(user_login) if user_type == 'nauczyciel' else {}}
                response_json = json.dumps(response_data, ensure_ascii=False)
                return Response(response_json, content_type="application/json; charset=utf-8")
            elif result[0] == "admin":
                    user_type = result[0]
                    response_data = {'type': user_type, 'data': admin.zadanie.dane(user_login) if user_type == 'admin' else {}}
                    response_json = json.dumps(response_data, ensure_ascii=False)
                    return Response(response_json, content_type="application/json; charset=utf-8")
        else:
            response_data = False
            response_json = json.dumps(response_data, ensure_ascii=False)
            return Response(response_json, content_type="application/json; charset=utf-8")

    response_data = "Błąd połączenia z bazą danych"
    response_json = json.dumps(response_data, ensure_ascii=False)
    return Response(response_json, content_type="application/json; charset=utf-8")

@app.route('/uczen_na_tle_klasy', methods=['GET'])
def uczen_na_tle_klasy():
    przedmiot = request.args.get('przedmiot')
    klasa = request.args.get('klasa')
    user_login = request.args.get('login')

    response_data = uczen.zadanie.untk(klasa, przedmiot, user_login)
    response_json = json.dumps(response_data, ensure_ascii=False)
    return Response(response_json, content_type="application/json; charset=utf-8")

@app.route('/zwroc_oceny_ucznia', methods=['GET'])
def zwroc_oceny_ucznia():
    klasa = request.args.get('klasa')
    user_login = request.args.get('login')
    nauczyciel = request.args.get('nauczyciel')

    response_data = uczen.zadanie.zou(klasa, user_login, nauczyciel)
    response_json = json.dumps(response_data, ensure_ascii=False)
    return Response(response_json, content_type="application/json; charset=utf-8")

@app.route('/usun_ocene', methods=['DELETE'])
def usun_ocene():
    opis = request.args.get('opis')
    ocena = request.args.get('ocena')
    klasa = request.args.get('klasa')
    user_login = request.args.get('login')
    nauczyciel_login = request.args.get('nauczyciel')
    przedmiot = request.args.get('przedmiot')
    data = request.args.get('data')

    response_data = nauczyciel.zadanie.usun_ocene(opis, ocena, klasa, user_login, nauczyciel_login, przedmiot, data)
    return jsonify(success=response_data)

@app.route('/edytuj_ocene', methods=['PUT'])
def edytuj_ocene():
    data = request.json
    opis = data['opis']
    ocena = data['ocena']
    klasa = data['klasa']
    user_login = data['login']
    nauczyciel_login = data['nauczyciel']
    przedmiot = data['przedmiot']
    data_wystawienia = data['data']
    nowa_ocena = data['nowa_ocena']

    response_data = nauczyciel.zadanie.edytuj_ocene(opis, ocena, klasa, user_login, nauczyciel_login, przedmiot, data_wystawienia, nowa_ocena)
    return jsonify(success=response_data)

@app.route('/wystaw_ocene', methods=['POST'])
def wystaw_ocene():
    data = request.json
    uczen_login = data['uczen_login']
    wystawil = data['wystawil']
    ocena = data['ocena']
    przedmiot = data['przedmiot']
    klasa = data['klasa']
    opis = data['opis']
    wystawil_login = data['wystawil_login']

    response_data = nauczyciel.zadanie.wystaw_ocene(uczen_login, wystawil, ocena, przedmiot, klasa, opis, wystawil_login)
    return jsonify(success=response_data)

@app.route('/zwroc_uwagi_ucznia', methods=['GET'])
def zwroc_uwagi_ucznia():
    user_login = request.args.get('login')

    response_data = uczen.zadanie.pobierz_uwagi_i_osiagniecia(user_login)
    response_json = json.dumps(response_data, ensure_ascii=False)
    return Response(response_json, content_type="application/json; charset=utf-8")

@app.route('/zwroc_frekwencje_ucznia', methods=['GET'])
def zwroc_frekwencje_ucznia():
    user_login = request.args.get('login')

    response_data = uczen.zadanie.pobierz_frekwencje(user_login)
    response_json = json.dumps(response_data, ensure_ascii=False)
    return Response(response_json, content_type="application/json; charset=utf-8")

@app.route('/usun_wydarzenie', methods=['DELETE'])
def usun_wydarzenie():
    nauczyciel_login = request.args.get('wystawil_login')
    data = request.args.get('data')
    termin = request.args.get('termin')
    przedmiot = request.args.get('przedmiot')
    opis = request.args.get('opis')
    typ = request.args.get('typ')
    klasa = request.args.get('klasa')

    response_data = nauczyciel.zadanie.usun_wydarzenie(nauczyciel_login, data, termin, przedmiot, opis, typ, klasa)
    return jsonify(success=response_data)

@app.route('/edytuj_wydarzenie', methods=['PUT'])
def edytuj_wydarzenie():
    nauczyciel_login = request.args.get('wystawil_login')
    data = request.args.get('data')
    termin = request.args.get('termin')
    przedmiot = request.args.get('przedmiot')
    opis = request.args.get('opis')
    typ = request.args.get('typ')
    klasa = request.args.get('klasa')

    nowy_termin = request.args.get('nowy_termin')
    nowy_opis = request.args.get('nowy_opis')
    nowy_typ = request.args.get('nowy_typ')

    response_data = nauczyciel.zadanie.edytuj_wydarzenie(nauczyciel_login, data, termin, przedmiot, opis, typ, klasa, nowy_termin, nowy_opis, nowy_typ)
    return jsonify(success=response_data)

@app.route('/utworz_wydarzenie', methods=['POST'])
def utworz_wydarzenie():
    nauczyciel_login = request.form.get('wystawil_login')
    nauczyciel_in = request.form.get('wystawil')
    termin = request.form.get('termin')
    przedmiot = request.form.get('przedmiot')
    opis = request.form.get('opis')
    typ = request.form.get('typ')
    klasa = request.form.get('klasa')

    response_data = nauczyciel.zadanie.utworz_wydarzenie(nauczyciel_login, nauczyciel_in, termin, przedmiot, opis, typ, klasa)
    return jsonify(success=response_data)

@app.route('/usun_uwage', methods=['DELETE'])
def usun_uwage():
    nauczyciel_login = request.args.get('wystawil_login')
    uczen_login = request.args.get('uczen_login')
    data = request.args.get('data')
    tresc = request.args.get('tresc')
    typ = request.args.get('typ')
    klasa = request.args.get('klasa')

    response_data = nauczyciel.zadanie.usun_uwage(nauczyciel_login, uczen_login, data, tresc, typ, klasa)
    return jsonify(success=response_data)

@app.route('/edytuj_uwage', methods=['PUT'])
def edytuj_uwage():
    nauczyciel_login = request.args.get('wystawil_login')
    uczen_login = request.args.get('uczen_login')
    data = request.args.get('data')
    tresc = request.args.get('tresc')
    typ = request.args.get('typ')
    klasa = request.args.get('klasa')

    nowa_tresc = request.args.get('nowa_tresc')
    nowy_typ = request.args.get('nowy_typ')

    response_data = nauczyciel.zadanie.edytuj_uwage(nauczyciel_login, uczen_login, data, tresc, typ, klasa, nowa_tresc, nowy_typ)
    return jsonify(success=response_data)

@app.route('/utworz_uwage', methods=['POST'])
def utworz_uwage():
    nauczyciel_login = request.form.get('wystawil_login')
    nauczyciel_in = request.form.get('wystawil')
    uczen_login = request.form.get('uczen_login')
    uczen = request.form.get('uczen')
    tresc = request.form.get('tresc')
    typ = request.form.get('typ')
    klasa = request.form.get('klasa')

    response_data = nauczyciel.zadanie.utworz_uwage(nauczyciel_login, uczen_login, uczen, tresc, typ, klasa, nauczyciel_in)
    return jsonify(success=response_data)

@app.route('/dodaj_frekwencje', methods=['POST'])
def dodaj_frekwencje():
    przedmiot = request.form.get('przedmiot')
    typ = request.form.get('typ')
    login = request.form.get('login')
    uczen = request.form.get('uczen')
    klasa = request.form.get('klasa')

    response_data = nauczyciel.zadanie.zapisz_frekwencje(przedmiot, typ, login, uczen, klasa)
    return jsonify(success=response_data)

@app.route('/pobierz_frekwencje', methods=['GET'])
def pobierz_frekwencje():
    login = request.args.get('login')

    response_data = nauczyciel.zadanie.pobierz_frekwencje(login)
    return jsonify(frekwencja=response_data)

@app.route('/usun_frekwencje', methods=['DELETE'])
def usun_frekwencje():
    przedmiot = request.args.get('przedmiot')
    typ = request.args.get('typ')
    login = request.args.get('login')
    uczen = request.args.get('uczen')
    klasa = request.args.get('klasa')
    data = request.args.get('data')

    response_data = nauczyciel.zadanie.usun_frekwencje(przedmiot, typ, login, uczen, klasa, data)
    return jsonify(success=response_data)

@app.route('/edytuj_frekwencje', methods=['PUT'])
def edytuj_frekwencje():
    data = request.form
    przedmiot = data.get('przedmiot')
    typ = data.get('typ')
    nowy_typ = data.get('nowy_typ')
    login = data.get('login')
    uczen = data.get('uczen')
    klasa = data.get('klasa')
    data = data.get('data')

    response_data = nauczyciel.zadanie.edytuj_frekwencje(przedmiot, typ, nowy_typ, login, uczen, klasa, data)
    return jsonify(success=response_data)

@app.route('/usun_konto', methods=['DELETE'])
def usun_konto():
    login = request.args.get('login')

    response_data = admin.zadanie.usun_konto(login)
    return jsonify(frekwencja=response_data)

@app.route('/utworz_ucznia', methods=['POST'])
def utworz_ucznia():
    login = request.form.get('login')
    imie_nazwisko = request.form.get('imie_nazwisko')
    haslo = request.form.get('haslo')
    klasa = request.form.get('klasa')
    ocena_z_zachowania = '6.0' #domyślna ocena z zachowania

    response_data = admin.zadanie.utworz_ucznia(login, imie_nazwisko, haslo, klasa, ocena_z_zachowania)
    return jsonify(success=response_data)

@app.route('/edytuj_ucznia', methods=['POST'])
def edytuj_ucznia():
    login = request.form.get('login')
    nowe_imie_nazwisko = request.form.get('imie_nazwisko')
    nowa_klasa = request.form.get('klasa')
    
    wynik = admin.zadanie.aktualizuj_ucznia(login, nowe_imie_nazwisko, nowa_klasa)
    return jsonify(success=wynik)

@app.route('/utworz_nauczyciela', methods=['POST'])
def utworz_nauczyciela():
    login = request.form.get('login')
    imie_nazwisko = request.form.get('imie_nazwisko')
    haslo = request.form.get('haslo')
    klasa = request.form.get('klasa')
    przedmioty = request.form.get('przedmioty')

    response_data = admin.zadanie.utworz_nauczyciela(login, imie_nazwisko, haslo, klasa, przedmioty)
    return jsonify(success=response_data)

@app.route('/edytuj_nauczyciela', methods=['POST'])
def edytuj_nauczyciela():
    login = request.form.get('login')
    nowe_imie_nazwisko = request.form.get('imie_nazwisko')
    nowa_klasa = request.form.get('klasa')
    nowe_przedmioty = request.form.get('przedmioty')

    wynik = admin.zadanie.aktualizuj_nauczyciela(login, nowe_imie_nazwisko, nowa_klasa, nowe_przedmioty)
    return jsonify(success=wynik)

@app.route('/utworz_admina', methods=['POST'])
def utworz_admina():
    login = request.form.get('login')
    haslo = request.form.get('haslo')

    response_data = admin.zadanie.utworz_admina(login, haslo)
    return jsonify(success=response_data)


if __name__ == '__main__':
    app.run(debug=True)
