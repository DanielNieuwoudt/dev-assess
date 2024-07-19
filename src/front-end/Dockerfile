# syntax=docker/dockerfile:1.2
# Use the official Node.js image as the base image
FROM node:18.20.3 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json files
COPY package*.json ./

# Install the dependencies
RUN npm install

# Copy the rest of the application code to the working directory
COPY . .

# Run the tests
RUN npm run test -- --watchAll=false

# Build the React app for production
RUN npm run build

FROM node:18.20.3

# Set the working directory
WORKDIR /app

# Copy the build output
COPY --from=build /app/build /app/build

# Copy the server code
COPY server.js /app

# Install server dependencies
RUN npm install express

# Write environment variable to .env file
ENV REACT_APP_ENVIRONMENT_NAME=${REACT_APP_ENVIRONMENT_NAME}

# Expose port 3000 to the outside world
EXPOSE 3000

# Start the Node.js server
CMD ["node", "server.js"]