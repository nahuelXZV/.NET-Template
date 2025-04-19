class BlazorHandler {
    maximumRetryCount;
    retryIntervalMilliseconds;
    reconnectModal;
    messageContainer;
    isCanceled;

    currentReconnectionProcess;

    blazorOptions;

    constructor(maximumRetryCount, retryIntervalMilliseconds) {
        this.maximumRetryCount = maximumRetryCount;
        this.retryIntervalMilliseconds = retryIntervalMilliseconds;
        this.reconnectModal = document.getElementById("reconnectBlazor");

        this.messageContainer = this.reconnectModal.querySelector(".message-container");

        this.isCanceled = false;

        this.currentReconnectionProcess = null;
    }

    async reconnect() {
        try {
            const result = await Blazor.reconnect();
            console.debug("Reconnect result: ", result)
            if (!result) {
                // The server was reached, but the connection was rejected; reload the page.
                location.reload();
                return;
            }

            // Successfully reconnected to the server.
            return;
        } catch (e) {

            // Si error es por Unauthorized, redirigir a login o hacer refresh
            const isUnauthorized = e.message.includes("Unauthorized");
            if (isUnauthorized) {
                location.reload();
            }

            console.error("ocurrió un error: ", e);
        }
    }

    async tryReconnection() {
        for (let i = 0; i < this.maximumRetryCount; i++) {
            this.messageContainer.innerText = `Attempting to reconnect: ${i + 1} of ${this.maximumRetryCount}`;

            await new Promise(resolve => setTimeout(resolve, this.retryIntervalMilliseconds));

            if (this.isCanceled) {
                return;
            }

            await this.reconnect();
        }

        // Retried too many times; reload the page.
        location.reload();
    }

    async reconnectionProcess() {
        this.reconnectModal.style.display = 'block';

        this.isCanceled = false;

        await this.tryReconnection();

        return {
            cancel: () => {
                this.isCanceled = true;
                this.reconnectModal.style.display = 'none';
            },
        };
    }

    async onConnectionUp() {
        this.currentReconnectionProcess?.cancel();
        this.currentReconnectionProcess = null;
    }

    async onConnectionDown(defaultReconnectionOptions, error) {
        console.debug("defaultReconnectionOptions: ", defaultReconnectionOptions);
        console.error("error: ", error);
        this.currentReconnectionProcess ??= await this.reconnectionProcess()
    }

    async initialize() {

        const options = {
            reconnectionHandler: {
                onConnectionUp: this.onConnectionUp.bind(this),
                onConnectionDown: this.onConnectionDown.bind(this)
            }
        }

        await Blazor.start(options);
        console.debug("Blazor started");
    }
}

const blazorHandler = new BlazorHandler(3, 5000);
blazorHandler.initialize();