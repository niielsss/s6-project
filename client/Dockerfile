# Stage 1: Build the React app
FROM node:14 AS build

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the application
RUN npm run build

# # Stage 2: Serve the React app with Nginx
# FROM nginx:alpine

# # Copy the build output to Nginx's html directory
# COPY --from=build /app/build /usr/share/nginx/html

# # Expose port 80
# EXPOSE 80

# # Start Nginx
# CMD ["nginx", "-g", "daemon off;"]

# Stage 2: Serve the React app
FROM node:14-alpine

# Install serve - a lightweight static file server
RUN npm install -g serve

# Copy the build output to the server directory
COPY --from=build /app/build /app

# Expose port 5000
EXPOSE 80

# Start the server
CMD ["serve", "-s", "app", "-l", "80"]