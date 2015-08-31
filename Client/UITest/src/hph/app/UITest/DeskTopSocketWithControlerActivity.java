package hph.app.UITest;


import java.io.InputStream;
import java.io.PrintWriter;
import java.net.Socket;

import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Bundle;

import android.util.Log;
import android.view.Display;
import android.view.KeyEvent;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.view.View.OnClickListener;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.Toast;

public class DeskTopSocketWithControlerActivity extends Activity implements OnClickListener{
	private AppConfig config;
	String hostString="";
	int port=9999;
	//private FrameLayout layout;
	private LinearLayout toolBarLayout;
	private DesktopImageView dskImg;
	private Socket client;
	private Bitmap bitMap;
	private BitmapFactory.Options bmpFactoryOptions;
	//public static int dw=480;
	//public static int dh=800;
	private String widthString="";
	private String heighString="";
	private String quality="";
	private String showModelString="";
	private String outString="";
	private Thread myThread;

	private boolean keyboardIsRun=false;
	private  boolean CanRun=true;
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.requestWindowFeature(Window.FEATURE_NO_TITLE);//去掉标题栏
        this.getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);//去掉信息栏
        setContentView(R.layout.desktop);
        config=AppConfig.getAppConfig();
        hostString=config.getServerHost();
        port=config.getServerPort();
        //layout=(FrameLayout)findViewById(R.id.MainLayOut);
        toolBarLayout=(LinearLayout)findViewById(R.id.toolBar);
        //为按钮添加单击事件
        ImageButton mouselbButton=(ImageButton)findViewById(R.id.btnMouseLB);
        ImageButton mouserbButton=(ImageButton)findViewById(R.id.btnMouseRB);
        ImageButton showHideAnImageButton=(ImageButton)findViewById(R.id.btnShowHide);
        ImageButton keyboardImageButton=(ImageButton)findViewById(R.id.btnKeyboard);
        mouselbButton.setOnClickListener(this);
        mouserbButton.setOnClickListener(this);
        showHideAnImageButton.setOnClickListener(this);
        keyboardImageButton.setOnClickListener(this);
        /*dskImg=new DesktopImageView(this);
        dskImg.setLayoutParams(new ViewGroup.LayoutParams(LayoutParams.FILL_PARENT,LayoutParams.FILL_PARENT));
        dskImg.setAdjustViewBounds(false);
        dskImg.setScaleType(ScaleType.FIT_XY);
        layout.addView(dskImg);*/
        dskImg=(DesktopImageView)findViewById(R.id.DesktopView);
        Display currentDisplay = getWindowManager().getDefaultDisplay();  
        config.setScreenWidth(currentDisplay.getWidth());  
        config.setScreenHeight(currentDisplay.getHeight());  
        widthString=config.getScreenHeight()+"w";
        heighString=config.getScreenWidth()+"h";
        quality=config.getQuality();
        showModelString=config.getShowModel();
        outString="get|"+widthString+"|"+heighString+"|"+quality+"|"+showModelString+"|";
        
        
        bmpFactoryOptions = config.getBmpFactoryOptions();
        bmpFactoryOptions.inJustDecodeBounds = true;  
  
        int heightRatio = (int)Math.ceil(bmpFactoryOptions.outHeight/(float)config.getScreenHeight());  
        int widthRatio = (int)Math.ceil(bmpFactoryOptions.outWidth/(float)config.getScreenWidth());  
  
        Log.v("HEIGHRATIO", ""+heightRatio);  
        Log.v("WIDTHRATIO", ""+widthRatio);  
  
        if (heightRatio > 1 && widthRatio > 1)  
        {  
            bmpFactoryOptions.inSampleSize =  heightRatio > widthRatio ? heightRatio:widthRatio;  
        }  
        bmpFactoryOptions.inJustDecodeBounds = false; 
        
        myThread =new Thread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
				while(CanRun){
					try {
						getDesktopView();
						//Thread.sleep(10);
					} catch (Exception e) {
						// TODO: handle exception
					}
					
				}
			}
		});
        myThread.start();
    }
    private boolean isStart=false;
    public void onClick(View v) {
    	int id=v.getId();
    	switch (id) {
			case R.id.btnShowHide:
				Animation hyperspaceJumpAnimation =null;
				if (isStart) {
					hyperspaceJumpAnimation =AnimationUtils.loadAnimation(this, R.anim.expendanim);
					toolBarLayout.startAnimation(hyperspaceJumpAnimation);
					///ImageButton btn=(ImageButton)v;
					v.setBackgroundResource(R.drawable.hide);
				}else {
					hyperspaceJumpAnimation =AnimationUtils.loadAnimation(this, R.anim.hideanim);
					toolBarLayout.startAnimation(hyperspaceJumpAnimation);
					v.setBackgroundResource(R.drawable.show);
				}
				isStart=!isStart;
				break;
			case R.id.btnMouseLB:
				SimpleSocketHelper.sendString(hostString, port, Commands.mouseLBPressString);
				break;
			case R.id.btnMouseRB:
				SimpleSocketHelper.sendString(hostString, port, Commands.mouseRBPressString);
				break;		
			case R.id.btnKeyboard:
				if (keyboardIsRun) {
					SimpleSocketHelper.sendString(hostString, port, Commands.closeKeyboardString);
				}
				else {
					SimpleSocketHelper.sendString(hostString, port, Commands.openKeyboardString);	
				}
				keyboardIsRun=!keyboardIsRun;
				break;
			default:
				break;
		}
    	
	}
    
    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
    	String cmd="";
        if (keyCode == KeyEvent.KEYCODE_BACK && event.getRepeatCount() == 0) {
               // Do something.
               CanRun=false;
               this.finish();//直接调用杀死当前activity方法.
               return true;
        }
        else if(keyCode==KeyEvent.KEYCODE_VOLUME_UP) {
			cmd = "mouselbpress|0,0";
		}
        else if(keyCode==KeyEvent.KEYCODE_VOLUME_DOWN){
			cmd="mouserbpress|0,0";
		}
        try {
        	if (cmd=="") {
        		return super.onKeyDown(keyCode, event);
			}
        	SimpleSocketHelper.sendString(hostString, port, cmd);
    	  	return true;
		} catch (Exception e) {
			// TODO: handle exception
			return super.onKeyDown(keyCode, event);
		}
    } 
    
    String erroeMsg=null;
    private void getDesktopView()
    {
    	InputStream inputStream=null;
    	PrintWriter out=null;
    	try {
    		/*
    		client=new Socket(hostString,port);
    		inputStream=client.getInputStream();
    		out = new PrintWriter(client.getOutputStream(),true);
    		out.print(outString);
    		out.flush();
    		bytes=new byte[0];
    		bytes=ByteToInputStream.input2byte(inputStream);
    		inputStream.close();
    		bitMap = BitmapFactory.decodeByteArray(bytes, 0, bytes.length,bmpFactoryOptions);
    		*/
    		DesktopRecieveEvent event=new DesktopRecieveEvent();
    		SimpleSocketHelper.sendString(hostString, port, outString, event);
    		if (event.getData()!=null) {
    			bitMap =(Bitmap) event.getData();	
			}else {
				return ;
			}
    		DeskTopSocketWithControlerActivity.this.runOnUiThread(new Runnable() {
				
				@Override
				public void run() {
					try {
			        	dskImg.setImageBitmap(bitMap);
						//Drawable drawable = new BitmapDrawable(bitMap); 
						//layout.setBackgroundDrawable(drawable);
			        	client.close();
			        	
					} catch (Exception e) {
						// TODO: handle exception
					}
					
				}
			});
    		
		} catch (Exception e) {
			// TODO: handle exception
			erroeMsg=e.getMessage();
			DeskTopSocketWithControlerActivity.this.runOnUiThread(new Runnable() {
				
				@Override
				public void run() {
					// TODO Auto-generated method stub
					Toast.makeText(getApplicationContext(), erroeMsg, Toast.LENGTH_SHORT);
				}
			});
		}
    	finally{
    		try {
    			if (inputStream!=null) {
    				inputStream.close();
    			}
    			if (out!=null) {
    				out.close();
    			}
			} catch (Exception e2) {
				// TODO: handle exception
			}
    		
    	}
    }
    
    
}