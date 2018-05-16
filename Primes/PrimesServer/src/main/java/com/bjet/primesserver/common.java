package com.bjet.primesserver;

import com.sun.net.httpserver.HttpExchange;
import com.sun.net.httpserver.HttpHandler;

import java.io.*;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.URI;
import java.net.URLDecoder;
import java.util.*;
import java.util.regex.*;

public class common {

    public enum Pages {echoHeader, echoGet, echoPost, echoPut, expectedResults, getReport}

    private enum Types {delete, get, post, put}

    @SuppressWarnings("unchecked")
    public static void parseQuery(String query, Map<String, Object> parameters) throws UnsupportedEncodingException {

        if (query != null) {
            String pairs[] = query.split("[&]");

            for (String pair : pairs) {
                String param[] = pair.split("[=]");

                String key = null;
                String value = null;
                if (param.length > 0) {
                    key = URLDecoder.decode(param[0], System.getProperty("file.encoding"));
                }

                if (param.length > 1) {
                    value = URLDecoder.decode(param[1], System.getProperty("file.encoding"));
                }

                if (parameters.containsKey(key)) {
                    Object obj = parameters.get(key);
                    if (obj instanceof List<?>) {
                        List<String> values = (List<String>) obj;
                        values.add(value);
                    } else if (obj instanceof String) {
                        List<String> values = new ArrayList<String>();
                        values.add((String) obj);
                        values.add(value);
                        parameters.put(key, values);
                    }
                } else {
                    parameters.put(key, value);
                }
            }
        }
    }

    private static String ExtractQuerry(HttpExchange he) {
        URI requestedUri = he.getRequestURI();
        String query = requestedUri.getRawQuery();
        if (query == null) query = he.getRequestURI().toString();
        if (query.contains("/")) {
            int start = query.lastIndexOf("/");
            query = query.substring(start + 1, query.length());
        }
        return query;
    }

    public static String FindString(String text, String regex) {
        if (text.isEmpty() || regex.isEmpty()) return "";
        try {
            Pattern p = Pattern.compile(regex);
            Matcher m = p.matcher(text);
            m.find();
            return m.group(0);
        } catch (Exception e) {
            return null;
        }
    }

    private static void PrintCall(HttpExchange he, String page) {
        System.out.println(he.getRemoteAddress().toString()
                + " | url: " + he.getRequestURI()
                + " | handler: " + page + " | "
                + he.getRequestMethod());
    }

    public static class EchoGet implements HttpHandler {

        @Override
        public void handle(HttpExchange he) throws IOException {
            PrintCall(he, "EchoGet");
            // parse request
            Map<String, Object> parameters = new HashMap<String, Object>();
            String query = ExtractQuerry(he);
            parseQuery(query, parameters);
            // send response
            int min = 0;
            int max = 10000;

            if (parameters.size() > 0) {
                if (parameters.containsKey("min")) {
                    min = Integer.parseInt(parameters.get("min").toString());
                }
            }
            if (parameters.containsKey("max")) {
                max = Integer.parseInt(parameters.get("max").toString());
            }
            String response = Integer.toString(RandInt(min, max));
            he.sendResponseHeaders(200, response.length());
            OutputStream os = he.getResponseBody();
            os.write(response.getBytes());
            //try { Thread.sleep(200);
            //} catch (InterruptedException ie) {
            //    ie.printStackTrace();
            //}
            os.close();
        }

        private int RandInt(int min, int max) {
            return min + (int) (Math.random() * ((max - min) + 1));
        }

        private int RandIntProtected(int min, int max){
            Random ran = new Random();
            if (min > max) {
                throw new IllegalArgumentException("Cannot draw random int from invalid range [" + min + ", " + max + "].");
            }
            int diff = max - min;
            if (diff >= 0 && diff != Integer.MAX_VALUE) {
                return (min + ran.nextInt(diff + 1));
            }
            int i;
            do {
                i = ran.nextInt();
            } while (i < min || i > max);
            return i;
        }
    }

    public static class EchoPostPut implements HttpHandler {

        @Override
        public void handle(HttpExchange he) throws IOException {
            PrintCall(he, "EchoPostPut");
            // parse request
            Map<String, Object> parameters = new HashMap<String, Object>();
            InputStreamReader isr = new InputStreamReader(he.getRequestBody(), "utf-8");
            BufferedReader br = new BufferedReader(isr);
            String query = br.readLine();
            parseQuery(query, parameters);
            // send response
            String response = "";
            for (String key : parameters.keySet())
                response += key + " = " + parameters.get(key) + "\n";
            he.sendResponseHeaders(200, response.length());
            OutputStream os = he.getResponseBody();
            os.write(response.toString().getBytes());
            os.close();

        }
    }
}
