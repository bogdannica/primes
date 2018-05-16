package com.bjet.primesserver;

public class Main
{
    public static int port = 8090;
    public static void main( String[] args ) {
        //a way to pass the port as argument
        if (args != null && args.length > 0) {
            try {
                port = Integer.parseInt(args[0]);
            } catch (NumberFormatException e) {
                System.err.println("Argument" + args[0] + " must be an integer.");
                System.exit(1);
            }
        }
        PrimeServer srvr = new PrimeServer();
        srvr.Start(port);

    }


    private static void displayImg(String path, int resolution)
    {
        Picture picture = new Picture(path);
        //"%d-by-%d\n", picture.width(), picture.height());
        String[] pattern = {"@","0","*","'","."};
        //System.out.printf(picture.toString(pattern, 100));
        picture.printimg(resolution);

    }
}
