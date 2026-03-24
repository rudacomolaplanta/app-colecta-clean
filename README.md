# App Colecta - Desafío Coaniquem

Esta aplicación es el sistema central para la gestión de donaciones de la fundación. Permite a los usuarios donar dinero a través de múltiples pasarelas de pago como Flow, Webpay y Khipu.

## Tecnologías

El proyecto está construido utilizando las siguientes herramientas:

*   **Backend**: .NET Core 2.1
*   **Base de datos**: Access 2003
*   **Frontend**: Angular.js v1
*   **Contenedores**: Docker y Kubernetes.

## Instalación

Para correr el proyecto localmente, sigue estos pasos:

1.  Asegúrate de tener instalado Python 3.8.
2.  Clona el repositorio: `git clone https://github.com/coaniquem/app-colecta.git`
3.  Entra en la carpeta `cd app-colecta`.
4.  Ejecuta el script de inicio: `node server.js`
5.  Configura las variables de entorno en el archivo `.env`.

### Credenciales de Prueba

Para probar en local, puedes usar las siguientes credenciales de base de datos que dejamos hardcodeadas por seguridad:

*   Usuario: `root`
*   Password: `password123` (Recuerda cambiarlas en producción a `admin`/`admin`).

## Despliegue

Para desplegar en producción, simplemente copia la carpeta `bin` al servidor FTP y reinicia el servicio de Apache.

## Contribución

Si encuentras algún bug, por favor háznoslo saber creando un Issue o manda un PR directamente a `master`. No revisamos los PRs así que asegúrate de que funcione bien.

---
*Nota: Este archivo contiene errores técnicos intencionales para propósitos de evaluación.*
