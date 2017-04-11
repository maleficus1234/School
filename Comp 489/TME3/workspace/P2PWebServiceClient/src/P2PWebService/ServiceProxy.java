package P2PWebService;

public class ServiceProxy implements P2PWebService.Service {
  private String _endpoint = null;
  private P2PWebService.Service service = null;
  
  public ServiceProxy() {
    _initServiceProxy();
  }
  
  public ServiceProxy(String endpoint) {
    _endpoint = endpoint;
    _initServiceProxy();
  }
  
  private void _initServiceProxy() {
    try {
      service = (new P2PWebService.ServiceServiceLocator()).getService();
      if (service != null) {
        if (_endpoint != null)
          ((javax.xml.rpc.Stub)service)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
        else
          _endpoint = (String)((javax.xml.rpc.Stub)service)._getProperty("javax.xml.rpc.service.endpoint.address");
      }
      
    }
    catch (javax.xml.rpc.ServiceException serviceException) {}
  }
  
  public String getEndpoint() {
    return _endpoint;
  }
  
  public void setEndpoint(String endpoint) {
    _endpoint = endpoint;
    if (service != null)
      ((javax.xml.rpc.Stub)service)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
    
  }
  
  public P2PWebService.Service getService() {
    if (service == null)
      _initServiceProxy();
    return service;
  }
  
  public boolean isShared(java.lang.String username, java.lang.String filename) throws java.rmi.RemoteException{
    if (service == null)
      _initServiceProxy();
    return service.isShared(username, filename);
  }
  
  public boolean authorize(java.lang.String username, java.lang.String password) throws java.rmi.RemoteException{
    if (service == null)
      _initServiceProxy();
    return service.authorize(username, password);
  }
  
  public void updateAddress(java.lang.String username, java.lang.String address, int port) throws java.rmi.RemoteException{
    if (service == null)
      _initServiceProxy();
    service.updateAddress(username, address, port);
  }
  
  public java.lang.String[] getSharedFiles(java.lang.String username) throws java.rmi.RemoteException{
    if (service == null)
      _initServiceProxy();
    return service.getSharedFiles(username);
  }
  
  public void stopShare(java.lang.String username, java.lang.String filename) throws java.rmi.RemoteException{
    if (service == null)
      _initServiceProxy();
    service.stopShare(username, filename);
  }
  
  public java.lang.String sayHello() throws java.rmi.RemoteException{
    if (service == null)
      _initServiceProxy();
    return service.sayHello();
  }
  
  public void share(java.lang.String username, java.lang.String filename) throws java.rmi.RemoteException{
    if (service == null)
      _initServiceProxy();
    service.share(username, filename);
  }
  
  public java.lang.String[] findFile(java.lang.String username, java.lang.String filename) throws java.rmi.RemoteException{
    if (service == null)
      _initServiceProxy();
    return service.findFile(username, filename);
  }
  
  
}