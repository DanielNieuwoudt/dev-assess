# syntax=docker/dockerfile:1.2
# Use the official Node.js image as the base image
FROM node:18.20.3

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json files
COPY package*.json ./

# Install the dependencies
RUN npm install

# Install serve globally
RUN npm install -g serve

# Copy the rest of the application code to the working directory
COPY . .

# Write environment variable to .env file
ARG REACT_APP_ENVIRONMENT_NAME
RUN echo "REACT_APP_ENVIRONMENT_NAME=${REACT_APP_ENVIRONMENT_NAME}" > .env

# Run the tests
RUN npm run test -- --watchAll=false

# Build the React app for production
RUN npm run build

# Expose port 3000 to the outside world
EXPOSE 3000

# Command to run the app
CMD ["serve", "-s", "build", "-l", "3000"]