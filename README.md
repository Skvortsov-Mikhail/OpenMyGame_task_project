# OpenMyGame_task_project
Тестовое задание от компании OpenMyGame для поступления на стажировку

### Мои комментарии проверяющим работу по заданиям
1. Задание **Fillwords** - уровни 7 и 8 идентичны, т.к. в тех.задании не было требования исключить повторные уровни.

2. Задание **Chess** - по тех.заданию есть условие: "для анимации вычисляется путь, состоящий из клеток, по которым фигура может последовательно прийти в точку назначения. Если пути нет, возвращается null;" При возвращении path = null происходит ошибка в SystemPieceMove.Update(). Ошибка связана с тем, что пока есть запрос на путь, он запрашивается, но при возвращаемом null запрос не удовлетворяется и запрашивается вновь.
Решить эту проблему необходимо в соответствии с пунктом тех.задания: "приложение должно работать без ошибок;", но её невозможно решить из метода ChessGridNavigator.FindPath(), в котором необходимо делать изменения.
Ошибка исправлена мной в классе SystemPieceMove путём добавления нескольких строк кода в  методах Update() и ProcessMove(), несмотря на условие тех.задания: "НЕ МЕНЯТЬ уже написанный код. Можно только дописывать свой код: методы, классы и тд. Сигнатуры методов уже написанного кода НЕ МЕНЯТЬ;"
К сожалению, других вариантов исправить ошибку, не нарушая условия "не менять уже написанный код", я не нашёл.

### Само тестовое задание представлено [здесь](https://drive.google.com/drive/folders/18yt8HEpZ71q5SQ6UFgoTDZmy0kLHgM5p?usp=sharing).
