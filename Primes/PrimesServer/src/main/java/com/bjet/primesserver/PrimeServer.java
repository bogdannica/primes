package com.bjet.primesserver;

import java.io.IOException;
import java.net.InetSocketAddress;
import com.sun.net.httpserver.HttpExchange;
import com.sun.net.httpserver.HttpHandler;
import com.sun.net.httpserver.HttpServer;

public class PrimeServer {
    private int port;
    private HttpServer server;

    public void Start(int port) {
        try {
            this.port = port;
            server = HttpServer.create(new InetSocketAddress(port), 0);

            System.out.println("HTTP SERVER started at " + port);
            server.createContext("/" + common.Pages.echoGet, new common.EchoGet());
            server.createContext("/" + common.Pages.echoPost, new common.EchoPostPut());
            server.createContext("/" + common.Pages.echoPut, new common.EchoPostPut());
            server.setExecutor(null);
            server.start();

        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void Stop() {
        server.stop(0);
        System.out.println("server stopped");
    }

}
