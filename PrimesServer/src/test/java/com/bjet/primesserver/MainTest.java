package com.bjet.primesserver;

import static org.junit.Assert.assertTrue;

import org.junit.Test;

/**
 * Unit test for simple App.
 */
public class MainTest
{
    @Test
    public void createServer()
    {
        try{PrimeServer srvr = new PrimeServer();}
        catch(Exception ex){throw ex;}
    }

}
