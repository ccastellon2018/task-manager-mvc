# 📌 Proyecto MVC - Task Manager

Este proyecto fue desarrollado como parte de mi aprendizaje en el curso del profesor **Felipe Gavilan** en la plataforma UDEMY, utilizando la última versión de .NET (10.0 LTS). El objetivo es construir una aplicación MVC para el manejo de tareas, integrando conceptos modernos de desarrollo web con ASP.NET Core.

## 🚀 Tecnologías utilizadas

* ASP.NET Core MVC (.NET 10.0 LTS)

* Entity Framework Core (EF Core)

* Identity para autenticación y autorización

* SQL Server como base de datos

* Internationalization (i18n) para soporte multilenguaje

* Visual Studio Community y CLI de .NET para desarrollo y migraciones

## 🎯 Objetivos de aprendizaje

* Comprender el patrón MVC y aplicarlo en un proyecto real.

* Implementar Identity para gestionar usuarios, roles y seguridad.

* Configurar DbContext y definir entidades con relaciones en C#.

* Usar OnModelCreating para establecer parámetros y restricciones en las tablas.

* Realizar migraciones con EF Core usando comandos del CLI y la terminal de Visual Studio.

* Implementar internacionalización para que la aplicación soporte múltiples idiomas.

## 📂 Estructura del proyecto

* Controllers/ → Controladores MVC para manejar la lógica de negocio.

* Models/ → Clases para proporcionar la data en las vistas, para el ususario final.
 
* Views/ → Vistas Razor para la interfaz de usuario.

* Entities/ → Entidades y clases de dominio (ej. Task, User)
 
* Infraestructure/ → Configuración de DbContext y migraciones de EF Core.
 
* Resources/ → Archivos de localización para internacionalización.

## 🛠️ Migraciones con EF Core e Instalaciones de Paquetes

```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

```
dotnet ef migrations add InitialCreate  
```

```
dotnet ef database update
```

## 🌍 Internacionalización

La aplicación soporta múltiples idiomas mediante archivos de recursos (.resx) y configuración en Program.cs para habilitar RequestLocalization.