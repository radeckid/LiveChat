export class Collection<T> {
  size: number;
  collection: Array<T>;

  constructor(size: number = 50) {
    this.size = size;
    this.collection = new Array<T>();
  }

  setSize(size: number) {
    this.size = size;
  }

  get(): Array<T> {
    return this.collection;
  }

  add(object: T) {
    const temp: Array<T> = new Array();

    while (this.getSize() > this.size) {
      delete this.collection[0];
    }

    for (let i = 0; i < this.getSize(); i++) {
      temp.push(this.collection[i]);
    }

    temp.push(object);
    this.collection = temp;
  }

  private getSize(): number {
    return this.collection.length;
  }
}
