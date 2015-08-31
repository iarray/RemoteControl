package hph.app.UITest;
enum FileType{
	Disk,Dir,File
}
public class FileItem {
	private int id;
	private String path;
	private int imgsrc;
	private FileType fileType;
	private int size;
	public int getSize() {
		return size;
	}
	public void setSize(int size) {
		this.size = size;
	}
	public FileType getFileType() {
		return fileType;
	}
	public void setFileType(FileType fileType) {
		this.fileType = fileType;
	}
	public int getId() {
		return id;
	}
	public void setId(int id) {
		this.id = id;
	}
	public String getPath() {
		return path;
	}
	public void setPath(String path) {
		this.path = path;
	}
	public int getImgsrc() {
		return imgsrc;
	}
	public void setImgsrc(int imgsrc) {
		this.imgsrc = imgsrc;
	}
}
