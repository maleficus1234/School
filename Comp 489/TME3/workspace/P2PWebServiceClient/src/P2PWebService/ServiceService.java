/**
 * ServiceService.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package P2PWebService;

public interface ServiceService extends javax.xml.rpc.Service {
    public java.lang.String getServiceAddress();

    public P2PWebService.Service getService() throws javax.xml.rpc.ServiceException;

    public P2PWebService.Service getService(java.net.URL portAddress) throws javax.xml.rpc.ServiceException;
}
