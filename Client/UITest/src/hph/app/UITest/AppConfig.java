package hph.app.UITest;

import java.io.File;
import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.BitmapFactory;
import android.os.Environment;

public class AppConfig {
	enum ConnectModel{
		LocalNetwork,PublicNetwork
	}
	private  String serverHost="";
	private int serverPort;
	private int screenWidth=0;
	private int screenHeight=0;
	private int moveRange=1;
	private BitmapFactory.Options bmpFactoryOptions= new BitmapFactory.Options();
	private String saveDirPath;
	private boolean isConnected=false;
	private String keyString;
	private ConnectModel connectModel;
	
	public ConnectModel getConnectModel() {
		return connectModel;
	}

	public void setConnectModel(ConnectModel connectModel) {
		this.connectModel = connectModel;
	}

	public String getKeyString() {
		return keyString;
	}

	public void setKeyString(String keyString) {
		this.keyString = keyString;
	}

	public boolean isConnected() {
		return isConnected;
	}

	public void setConnected(boolean isConnected) {
		this.isConnected = isConnected;
	}

	public String getSaveDirPath() {
		return saveDirPath;
	}

	public void setSaveDirPath(String saveDirPath) {
		if (creatFolder(saveDirPath))
		{
			this.saveDirPath = saveDirPath;
		}
	}

	public BitmapFactory.Options getBmpFactoryOptions() {
		return bmpFactoryOptions;
	}

	public int getMoveRange() {
		return moveRange;
	}

	public void setMoveRange(int moveRange) {
		this.moveRange = moveRange;
	}

	private String quality;
	private String showModel;
	private String widthString;
	private String heightString;
	private int i_quality;
	
	private static AppConfig config;
	private AppConfig() {
		showModel="FollowMouse";
		quality="30q";
		i_quality=30;
		connectModel=ConnectModel.LocalNetwork;
		saveDirPath=Environment.getExternalStorageDirectory().getAbsolutePath()+"/口袋桌面接收文件/";
		creatFolder(saveDirPath);
	}
	
	private boolean creatFolder(String path) {
		File destDir = new File(path);
		  if (!destDir.exists()) {
		   return destDir.mkdirs();
		  }
		  return true;
	}
	
	public String getWidthString() {
		return widthString;
	}

	public String getHeightString() {
		return heightString;
	}

	public static AppConfig getAppConfig() {
		if (config==null) {
			config=new AppConfig();
			return config;
		}
		else {
			return config;
		}
	}

	public String getServerHost() {
		return serverHost;
	}

	public void setServerHost(String serverHost) {
		this.serverHost = serverHost;
	}

	public int getServerPort() {
		return serverPort;
	}

	public void setServerPort(int serverPort) {
		this.serverPort = serverPort;
	}

	public int getScreenWidth() {
		return screenWidth;
	}

	public String getQuality() {
		return quality;
	}
	
	public int getIntQuality() {
		return i_quality;
	}

	public void setQuality(int quality) {
		this.i_quality=quality;
		this.quality = quality+"q";
	}

	public String getShowModel() {
		return showModel;
	}

	public void setShowModel(String showModel) {
		this.showModel = showModel;
	}

	public void setScreenWidth(int screenWidth) {
		this.screenWidth = screenWidth;
		heightString=screenWidth+"h";
	}

	public int getScreenHeight() {
		return screenHeight;
	}

	public void setScreenHeight(int screenHeight) {
		this.screenHeight = screenHeight;
		widthString=screenHeight+"w";
	}
	
	public static void loadOptions(Context cont) {
	   SharedPreferences sp = cont.getSharedPreferences("Options", 0);
       AppConfig.getAppConfig().setSaveDirPath(sp.getString("SavePath", Environment.getExternalStorageDirectory().getAbsolutePath()+"/口袋桌面接收文件/"));
       AppConfig.getAppConfig().setShowModel(sp.getString("ShowModel", "FollowMouse"));
       AppConfig.getAppConfig().setMoveRange(sp.getInt("MouseMoveRange", 1));
       AppConfig.getAppConfig().setQuality(sp.getInt("ImageQuality", 30));
       AppConfig.getAppConfig().setServerPort(sp.getInt("ServerPort", 9999));
	}
}
