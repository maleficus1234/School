/**
 * Service.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package P2PWebService;

public interface Service extends java.rmi.Remote {
    public boolean isShared(java.lang.String username, java.lang.String filename) throws java.rmi.RemoteException;
    public boolean authorize(java.lang.String username, java.lang.String password) throws java.rmi.RemoteException;
    public void updateAddress(java.lang.String username, java.lang.String address, int port) throws java.rmi.RemoteException;
    public java.lang.String[] getSharedFiles(java.lang.String username) throws java.rmi.RemoteException;
    public void stopShare(java.lang.String username, java.lang.String filename) throws java.rmi.RemoteException;
    public java.lang.String sayHello() throws java.rmi.RemoteException;
    public void share(java.lang.String username, java.lang.String filename) throws java.rmi.RemoteException;
    public java.lang.String[] findFile(java.lang.String username, java.lang.String filename) throws java.rmi.RemoteException;
}
