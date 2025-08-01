FROM nginx:alpine

# Копируем наши файлы
COPY ./public /usr/share/nginx/html
COPY ./src/styles.css /usr/share/nginx/html/styles.css
COPY ./src/script.js /usr/share/nginx/html/script.js

EXPOSE 80