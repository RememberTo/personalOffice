# PersonalOffice Backend Core

## Развертывание проекта
### Консоль
1) Клонируем сам проект и все его submodule's  
`git clone --recurse-submodules http://gitlab.dev.solidbroker.ru/solid-application-foundation/personaloffice-backend-core.git`
2) Переходим в папку с submodul'ем
3) Переключаем на ветку ASP 
`git checkout ASP`
4) Обновляем submodule 
`git submodule update --recursive --remote`

### Visual Studio 2022
1) Клонируем сам проект и все его submodule'и  
```git clone --recurse-submodules http://gitlab.dev.solidbroker.ru/solid-application-foundation/personaloffice-backend-core.git```
2) Открываем вкладку Git Changes
3) Выбираем проект message-bus-core и выбраем remote ветку origin/ASP

### Дополнительно
- Клонирование проекта 
```git clone http://gitlab.dev.solidbroker.ru/solid-application-foundation/personaloffice-backend-core.git```
- Клонирование submodul'ей
```git submodule update --init --recursive```
---
## Зависимости
- NLog (5.2.7)
- Newtonsoft.Json (13.0.3)
- MediatR (12.2.0)
- FluentValidation (11.8.1)
- AutoMapper (12.0.1)
- RabbitMQ.Clinet (6.8.1)