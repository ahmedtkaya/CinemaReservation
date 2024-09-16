pipeline {
    agent any

    stages {
        stage('Clone Repository') {
            steps {
                git 'https://github.com/ahmedtkaya/CinemaReservation.git' // Git repo'nuzu belirtin
            }
        }

        stage('Build') {
            steps {
                script {
                    sh 'dotnet restore'
                    sh 'dotnet build --configuration Release'
                }
            }
        }

        stage('Test') {
            steps {
                script {
                    sh 'dotnet test'
                }
            }
        }

        stage('Publish') {
            steps {
                script {
                    sh 'dotnet publish -c Release -o out'
                }
            }
        }

        stage('Docker Build and Push') {
            steps {
                script {
                    sh 'docker build -t cinema-reservation .'
                    sh 'docker tag cinema-reservation ahmedtkaya/cinema-reservation:latest'
                    sh 'docker push ahmedtkaya/cinema-reservation:latest'
                }
            }
        }

        stage('Deploy') {
            steps {
                script {
                    // Deployment adımınız (Kubernetes, Docker Swarm, vb.) buraya eklenebilir
                    sh 'docker run -d -p 8080:80 ahmedtkaya/cinema-reservation:latest'
                }
            }
        }
    }
}
