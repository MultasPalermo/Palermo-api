pipeline {
    agent any

    environment {
        DOTNET_CLI_HOME = '/var/jenkins_home/.dotnet'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        DOTNET_NOLOGO = '1'
        PROJECT_PATH = 'taller/Web/Web.csproj'
    }

    stages {

        // =======================================================
        // 1️⃣ CHECKOUT
        // =======================================================
        stage('Checkout código fuente') {
            steps {
                echo "📥 Clonando repositorio desde GitHub..."
                checkout scm
                sh 'ls -R taller/DevOps || true'
            }
        }

        // =======================================================
        // 2️⃣ DETECTAR ENTORNO SEGÚN LA RAMA
        // =======================================================
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

        // =======================================================
        // 3️⃣ COMPILAR Y PUBLICAR .NET
        // =======================================================
        stage('Compilar .NET dentro de contenedor SDK') {
            steps {
                script {
                    docker.image('mcr.microsoft.com/dotnet/sdk:9.0')
                        .inside('-v /var/run/docker.sock:/var/run/docker.sock -u root:root') {
                        sh '''
                            echo "🔧 Restaurando dependencias .NET..."
                            cd taller
                            dotnet restore Web/Web.csproj
                            dotnet build Web/Web.csproj --configuration Release
                            dotnet publish Web/Web.csproj -c Release -o ./publish
                        '''
                    }
                }
            }
        }

        // =======================================================
        // 4️⃣ CONSTRUIR IMAGEN DOCKER
        // =======================================================
        stage('Construir imagen Docker') {
            steps {
                dir('taller') {
                    sh """
                        echo "🐳 Construyendo imagen Docker para alcaldiafetch (${env.ENVIRONMENT})"
                        docker build -t alcaldiafetch-api-${env.ENVIRONMENT}:latest -f Web/Dockerfile .
                    """
                }
            }
        }

        // =======================================================
        // 5️⃣ PREPARAR RED Y BASE DE DATOS
        // =======================================================
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

        // =======================================================
        // 6️⃣ DESPLEGAR API CON DOCKER COMPOSE
        // =======================================================
        stage('Desplegar API alcaldiafetch') {
            steps {
                dir('.') {
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
