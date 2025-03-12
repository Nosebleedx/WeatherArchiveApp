DROP TABLE IF EXISTS public."WeatherDatas"; 

CREATE TABLE public."WeatherDatas"
(
    "Id" SERIAL PRIMARY KEY, 
    "DateTime" date NOT NULL, 
    "Time" time without time zone NOT NULL, 
    "Temperature" real NOT NULL, 
    "Humidity" integer NOT NULL, 
    "DewPoint" real NOT NULL, 
    "Pressure" integer NOT NULL, 
    "WindDirection" character varying NOT NULL,
    "WindSpeed" integer NOT NULL,
    "Cloudiness" integer NOT NULL,
    "CloudBaseHeight" integer NOT NULL, 
    "Visibility" integer NULL, -- 
    "WeatherPhen" character varying NULL
);

SELECT * FROM public."WeatherDatas"
ORDER BY "Id" ASC 