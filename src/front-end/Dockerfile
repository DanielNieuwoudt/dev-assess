# Use the official Node.js image as the base image
FROM node:18

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

# Install serve to serve the build folder
RUN npm install -g serve

# Expose port 3000 to the outside world
EXPOSE 3000

# Command to run the app
CMD ["serve", "-s", "build", "-l", "3000"]