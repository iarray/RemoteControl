package hph.app.UITest;


import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import android.content.Context;
import android.util.AttributeSet;
import android.view.MotionEvent;
import android.widget.ImageView;

public class DesktopImageView extends ImageView {
		//String hostString="192.168.191.1";
		//int port=9999;
		private float startX=0;
		private float startY=0;
		private float x=0,y=0;
		private String cmdString;
		private ExecutorService pool = Executors.newFixedThreadPool(2); 
		private  Thread thread = new SendCmdThread();
	 public DesktopImageView(Context context) {
		  super(context);
		  // TODO Auto-generated constructor stub
		 }
		 public DesktopImageView(Context context,AttributeSet attrs) {
		  super(context,attrs);
		  // TODO Auto-generated constructor stub
		 }
		 public DesktopImageView(Context context,AttributeSet attrs,int defStyle) {
		  super(context,attrs,defStyle);
		  // TODO Auto-generated constructor stub
		 }

		 @Override
		 public boolean onTouchEvent(MotionEvent event) {
		  /* Absolute Point Move
		  int eventaction = event.getAction();
		  String cmdString=null;
		  float rawX = event.getRawX();
		  float rawY = event.getRawY();
		  switch(eventaction){
		  	  case MotionEvent.ACTION_DOWN:
		  		  //cmdString = "mouselbpress|"+(int)rawX+","+(int)rawY;
		  		  break;
			  case MotionEvent.ACTION_MOVE:
				  float x=rawY/DeskTopSocketWithControlerActivity.dh;
				  float y=rawX/DeskTopSocketWithControlerActivity.dw;
				  cmdString = "mousemove|"+(int)(x*1366)+","+(768-(int)(y*768));
			      break;
			  case MotionEvent.ACTION_UP:
				  break;
			  }
		  try {
			  if (cmdString!=null) {
				  Socket client=new Socket(hostString,port);
				  PrintWriter out = new PrintWriter(client.getOutputStream(),true);
		    	  out.print(cmdString);
		    	  out.close();
		    	  client.close();
				}
			} catch (Exception e) {
			// TODO: handle exception
		}
		*/
			 //Relatively Point Move
			 int moveRange=AppConfig.getAppConfig().getMoveRange();
			  int eventaction = event.getAction();
			  //long curDate=0,endDate=1000;
			  float curX, curY;
			  switch(eventaction){
			  	  case MotionEvent.ACTION_DOWN:
			  		 startX= event.getX();
			  		 startY=event.getY();
			  		 //curDate= System.currentTimeMillis();
				  break;
			  	  case MotionEvent.ACTION_MOVE:
					  curX = event.getX();
					  curY = event.getY();
					  x=(curY-startY);//DeskTopSocketWithControlerActivity.dh)*1366;
					  y=(startX-curX);//DeskTopSocketWithControlerActivity.dw)*766;
					  startX=curX;
					  startY=curY;
					  if(x!=0||y!=0){
						  cmdString = "mouseadmove|"+(int)(x*moveRange)+","+(int)y*moveRange;
					  }
					  break;
			  	  /*case MotionEvent.ACTION_UP:
			  		  endDate =System.currentTimeMillis();
			  		  long diff = endDate-curDate;
			  		Toast.makeText(getContext(),String.valueOf(diff),Toast.LENGTH_SHORT).show();
			  		  if (diff<200) {
						cmdString="mouselbpress|0,0";
					}
			  		  break;*/
				  }
			  if (cmdString!=null) {
				  pool.execute(thread);
			  }
			  //Toast.makeText(getContext(),String.valueOf(downtime),Toast.LENGTH_SHORT).show();
		  this.invalidate();
		  return true;
		 }
		 
		 public class SendCmdThread extends Thread{
		 
			 @Override
			public void run(){
				
				 	/* 
				 	 try {
						 
					  Socket client=new Socket(hostString,port);
					  PrintWriter out = new PrintWriter(client.getOutputStream(),true);
			    	  out.print(cmdString);
			    	  out.close();
			    	  client.close();
					} catch (Exception e) {
						// TODO: handle exception
					} 
					*/
				 SimpleSocketHelper.sendString(AppConfig.getAppConfig().getServerHost(), AppConfig.getAppConfig().getServerPort(), cmdString);
			 }
		 }
}