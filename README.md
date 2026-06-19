# Trabajo Practico N° 2 : Investigación + práctica
## Tema: SignalR
## 🎵 Plataforma de Intercambio y Subasta de Instrumentos Musicales

## Objetivo del Proyecto

Desarrollar una plataforma web de intercambio y subasta de instrumentos musicales que permita a los usuarios publicar instrumentos, participar en subastas y comunicarse mediante un chat en tiempo real.

La aplicación utiliza **SignalR** para brindar comunicación y notificaciones instantáneas entre los participantes, permitiendo realizar consultas y negociar directamente durante el proceso de la subasta.

Como característica principal, el sistema incorpora un mecanismo de **detección automática de ofertas** dentro del chat. Cuando un usuario envía un mensaje utilizando las palabras clave:

- **DINERO** *monto*
- **PERMUTO** *monto + descripción del instrumento u objeto*

el sistema interpreta automáticamente el contenido del mensaje, identifica el tipo de oferta y **crea el registro de la oferta de forma automática**, sin necesidad de que el usuario complete un formulario adicional. Esto hace que el proceso de ofertar sea más rápido, intuitivo y natural para los participantes.

## Funcionalidades principales

- Registro e inicio de sesión de usuarios.
- Publicación de instrumentos musicales.
- Chat en tiempo real mediante SignalR.
- Envío y recepción de notificaciones instantáneas.
- Detección automática de ofertas a partir de las palabras clave **DINERO** y **PERMUTO**.
- Creación automática de ofertas detectadas en el chat.
- Registro del historial de ofertas realizadas.
- Seguimiento del estado de las subastas y determinación de la oferta ganadora.

## Tecnologías utilizadas

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- SignalR
- HTML5
- CSS3
- Bootstrap
- JavaScript# TPinvestigacion
