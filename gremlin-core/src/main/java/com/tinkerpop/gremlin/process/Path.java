package com.tinkerpop.gremlin.process;

import org.javatuples.Pair;

import java.util.List;
import java.util.Set;
import java.util.function.BiConsumer;
import java.util.function.Consumer;
import java.util.stream.IntStream;
import java.util.stream.Stream;

/**
 * A Path denotes a particular walk through a {@link com.tinkerpop.gremlin.structure.Graph} as defined by a {@link Traverser}.
 * In abstraction, any Path implementation maintains two lists: a list of sets of labels and a list of objects.
 * The list of labels are the labels of the steps traversed.
 * The list of objects are the objects traversed.
 *
 * @author Marko A. Rodriguez (http://markorodriguez.com)
 */
public interface Path extends Cloneable {

    /**
     * Get the number of step in the path.
     *
     * @return the size of the path
     */
    public int size();

    /**
     * Add a new step to the path which includes a {@link Step} label and the object produced by that step.
     *
     * @param label  the label of the step
     * @param object the object generated by that step
     * @return the extended path
     */
    public Path extend(final String label, final Object object);

    /**
     * Add a new step to the path which includes a {@link Step} label and the object produced by that step.
     *
     * @param labels the label of the step
     * @param object the object generated by that step
     * @return the extended path
     */
    public Path extend(final Set<String> labels, final Object object);

    /**
     * Get the object associated with the particular label of the path.
     *
     * @param label the label of the path
     * @param <A>   the type of the object associated with the label
     * @return the object associated with the label of the path
     */
    public <A> A get(final String label);

    /**
     * Get the object associated with the specified index into the path.
     *
     * @param index the index of the path
     * @param <A>   the type of the object associated with the index
     * @return the object associated with the index of the path
     */
    public <A> A get(final int index);

    /**
     * Return true if the path has the specified label, else return false.
     *
     * @param label the label to search for
     * @return true if the label exists in the path
     */
    public boolean hasLabel(final String label);

    /**
     * Add a label to the current head of the path.
     *
     * @param label the label to add to the head of the path
     */
    public void addLabel(final String label);

    /**
     * An ordered list of the objects in the path.
     *
     * @return the objects of the path
     */
    public List<Object> objects();

    /**
     * An ordered list of the labels associated with the path
     *
     * @return the labels of the path
     */
    public List<Set<String>> labels();

    public Path clone();

    /**
     * Determines whether the path is a simple or not.
     * A simple path has no cycles and thus, no repeated objects.
     *
     * @return Whether the path is simple or not
     */
    public default boolean isSimple() {
        final List<Object> objects = this.objects();
        for (int i = 0; i < objects.size() - 1; i++) {
            for (int j = i + 1; j < objects.size(); j++) {
                if (objects.get(i).equals(objects.get(j)))
                    return false;
            }
        }
        return true;
    }

    public default void forEach(final Consumer<Object> consumer) {
        this.objects().forEach(consumer);
    }

    public default void forEach(final BiConsumer<Set<String>, Object> consumer) {
        final List<Set<String>> labels = this.labels();
        final List<Object> objects = this.objects();
        for (int i = 0; i < objects.size(); i++) {
            consumer.accept(labels.get(i), objects.get(i));
        }
    }

    public default Stream<Pair<Set<String>, Object>> stream() {
        final List<Set<String>> labels = this.labels();
        final List<Object> objects = this.objects();
        return IntStream.range(0, this.size()).mapToObj(i -> Pair.with(labels.get(i), objects.get(i)));
    }

    public static class Exceptions {

        public static IllegalArgumentException stepWithProvidedLabelDoesNotExist(final String label) {
            return new IllegalArgumentException("The step with label " + label + "  does not exist");
        }
    }
}
