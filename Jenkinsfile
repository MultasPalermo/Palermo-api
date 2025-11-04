pipeline {
    agent any

    environment {
        DOTNET_CLI_HOME = '/var/jenkins_home/.dotnet'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        DOTNET_NOLOGO = '1'
        PROJECT_PATH = 'taller/Web/Web.csproj'
    }

    stages {

        stage('Checkout código fuente') {
            steps {
                echo "📥 Clonando repositorio desde GitHub..."
                checkout scm
                sh 'pwd && ls -R || true'
            }
        }

        stage('Detectar entorno') {
            steps {
                script {
                    switch (env.BRANCH_NAME) {
                        case 'main': env.ENVIRONMENT = 'prod'; break
                        case 'staging': env.ENVIRONMENT = 'staging'; break
                        case 'qa': env.ENVIRONMENT = 'qa'; break
                        default: env.ENVIRONMENT = 'develop'; break
                    }

                    env.ENV_DIR = "taller/DevOps/${env.ENVIRONMENT}"
                    env.COMPOSE_FILE = "${env.ENV_DIR}/docker-compose.yml"
                    env.ENV_FILE = "${env.ENV_DIR}/.env"
                    env.DB_COMPOSE_FILE = "alcaldiaFetch-DB/docker-compose-db.yml"

                    echo """
                    ✅ Rama detectada: ${env.BRANCH_NAME}
                    🌎 Entorno asignado: ${env.ENVIRONMENT}
                    📄 Compose file (API): ${env.COMPOSE_FILE}
                    📁 Env file (API): ${env.ENV_FILE}
                    🗄 Compose file (DB): ${env.DB_COMPOSE_FILE}
                    """

                    if (!fileExists(env.COMPOSE_FILE)) {
                        error "❌ No se encontró ${env.COMPOSE_FILE}"
                    }
                }
            }
        }

        stage('Compilar .NET dentro de contenedor SDK') {
            steps {
                script {
                    docker.image('mcr.microsoft.com/dotnet/sdk:9.0')
                        .inside('-v /var/run/docker.sock:/var/run/docker.sock -u root:root') {

                        // 🔹 Ya no se instala docker.io (causaba conflicto con el binario montado)
                        // Jenkins ya usa el Docker del host gracias al socket compartido.

                        sh """
                            echo "🔧 Restaurando dependencias .NET..."
                            dotnet restore ${PROJECT_PATH}

                            echo "🏗 Compilando proyecto..."
                            dotnet build ${PROJECT_PATH} --configuration Release

                            echo "📦 Publicando artefactos..."
                            dotnet publish ${PROJECT_PATH} -c Release -o ./publish
                        """

                        sh 'ls -R ./publish || true'
                    }
                }
            }
        }

        stage('Construir imagen Docker') {
            steps {
                sh """
                    echo "🐳 Construyendo imagen Docker para entorno: ${env.ENVIRONMENT}"
                    docker build -t alcaldiafetch-api-${env.ENVIRONMENT}:latest -f taller/Web/Dockerfile .
                """
            }
        }

        stage('Preparar red y base de datos') {
            steps {
                script {
                    sh """
                        echo "🌐 Creando red externa compartida (si no existe)..."
                        docker network create alcaldiafetch_network || true

                        echo "🗄 Levantando stack de bases de datos..."
                        docker compose -f ${env.DB_COMPOSE_FILE} up -d
                    """
                }
            }
        }

        stage('Desplegar API alcaldiafetch') {
            steps {
                script {
                    sh """
                        echo "🚀 Desplegando entorno: ${env.ENVIRONMENT}"
                        docker compose -f ${env.COMPOSE_FILE} --env-file ${env.ENV_FILE} up -d --build
                    """
                }
            }
        }
    }

    post {
        success {
            echo "🎉 Despliegue completado correctamente para ${env.ENVIRONMENT}"
        }
        failure {
            echo "💥 Error durante el despliegue en ${env.ENVIRONMENT}"
        }
        always {
            echo "🧹 Limpieza final del pipeline completada."
        }
    }
}
