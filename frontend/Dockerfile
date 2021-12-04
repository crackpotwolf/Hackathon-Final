# Stage 1: Compile and Build angular codebase

# Use official node image as the base image
FROM node:16-alpine as build

# Set the working directory
WORKDIR /app

RUN export NODE_OPTIONS=--openssl-legacy-provider

# Add the source code to app
COPY /accelerator/ /app

RUN npm install --force

# Install all the dependencies
# RUN npm install

# Generate the build of the application
RUN npm run build


# Stage 2: Serve app with nginx server

# Use official nginx image as the base image
FROM nginx:latest

# Copy the build output to replace the default nginx contents.
COPY ./nginx/nginx.conf /etc/nginx/nginx.conf
COPY --from=build /app/dist/accelerator /usr/share/nginx/html